using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver
{
    public partial class DriverViewModel : BaseViewModel
    {

        public Command StartCommand { get; }
        private bool showChoosingRouteForm = true;
        private List<string> routeList = new List<string>();
        private int routeListSelectedIndex;
        private List<Dictionary<string,string>> routes = new List<Dictionary<string, string>>();
        private string vehicleID = "";

        public List<string> RouteList
        {
            get => routeList;
            set => SetProperty(ref routeList, value);
        }
        public int RouteListSelectedIndex
        {
            get => routeListSelectedIndex;
            set => SetProperty(ref routeListSelectedIndex, value);
        }

        public string VehicleID
        {
            get => vehicleID;
            set => SetProperty(ref vehicleID, value);
        }

        public bool ShowChoosingRouteForm
        {
            get => showChoosingRouteForm;
            set => SetProperty(ref showChoosingRouteForm, value);
        }


        private async Task initialiseAsync()
        {
            await LoadingPopupPage.ShowLoading();
            try
            {
                routes = await Models.Api.Route.GetAll(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""));
                var routesNames = (from route in routes select route["name"]).ToList();
                RouteListSelectedIndex = 1;
                RouteList = routesNames;
                RouteListSelectedIndex = 0;
                await LoadingPopupPage.HideLoadingAsync();
            }
            catch (Exception ex)
            {
                await LoadingPopupPage.HideLoadingAsync();
            }
        }

        public async void OnStartClicked()
        {
            //Sacamos y comprobamos todos los datos
            var vehicleId = VehicleID.Trim();
            if (String.IsNullOrEmpty(vehicleId))
            {
                await this._messageService.DisplayAlert(Resources.GeneralTexts.Error, Resources.DriverTexts.ErrorVehicleIdEmpty, Resources.GeneralTexts.Close);
                return;
            }
            if(routes.Count < RouteListSelectedIndex)
            {
                await this._messageService.DisplayAlert(Resources.GeneralTexts.Error, Resources.DriverTexts.ErrorLocatingRoute, Resources.GeneralTexts.Close);
                return;
            }
            var routeData = routes[RouteListSelectedIndex];


            //ponemos el waiting
            await LoadingPopupPage.ShowLoading();
            //enviamos la asociación con el conductor
            try
            {
                await Models.Api.User.UpdateDriverVehicleAndRoute(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), VehicleID, routeData["id"]);
            }
            catch (Exception ex)
            {
                await this._messageService.DisplayAlert(Resources.GeneralTexts.Error, Resources.GeneralTexts.ConnectionError, Resources.GeneralTexts.Close);
                await LoadingPopupPage.HideLoadingAsync();
                return;
            }
            //TODO inicializamos los listener y servicios
            //quitamos el waiting
            await LoadingPopupPage.HideLoadingAsync();
            //mostramos la pantalla de trabajo
            IsRunning = true;
        }

        
    }
}
