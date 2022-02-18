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
                await this._messageService.DisplayAlert("Error", "Vehicle id field cannot be empty", "Close");
                return;
            }
            if(routes.Count < RouteListSelectedIndex)
            {
                await this._messageService.DisplayAlert("Error", "Error locating the route", "Close");
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
                await this._messageService.DisplayAlert("Error", "Connexion error.", "Close");
                await LoadingPopupPage.HideLoadingAsync();
                return;
            }
            //TODO inicializamos los listener y servicios
            //quitamos el waiting
            await LoadingPopupPage.HideLoadingAsync();
            //mostramos la pantalla de trabajo
            IsRunning = true;
        }

        /*public async void CheckLoggedUser()
        {
            string userApiToken = Preferences.Get(Constants.PreferenceKeys.UserApiToken, "");
            if (!string.IsNullOrEmpty(userApiToken))
            {
                await LoadingPopupPage.ShowLoading();
                //Si hay token, comprobamos si es valido, de ser asi enviamos a la pagina principal. Si no, lo borramos
                try
                {
                    //Try to update the device notification token on the same request
                    string currentNotificationToken = await NotificationHandler.Instance.GetTokenAsync();
                    //await Models.Api.User.GetUserAsync(userApiToken);
                    await Models.Api.User.UpdateNotificationTokenAsync(userApiToken, currentNotificationToken);
                    //Cargamos la pagina principal
                    await LoadingPopupPage.HideLoadingAsync();
                    (App.Current.MainPage as AppShell).SetLoggedUserContextAsync();
                    return;
                }
                catch (Exception ex)
                {
                    await LoadingPopupPage.HideLoadingAsync();
                    Preferences.Remove(Constants.PreferenceKeys.UserApiToken);
                }
            }
            ShowForm = true;
        }*/

        /*private async void OnLoginClicked(object obj)
        {

            string cleanUsername = username.Trim();
            string cleanPassword = password.Trim();
            if (string.IsNullOrEmpty(cleanUsername) || string.IsNullOrEmpty(cleanPassword))
            {
                return;
            }
            try
            {
                await LoadingPopupPage.ShowLoading();
                var loginResponse = await Models.Api.User.LoginAsync(cleanUsername, cleanPassword);
                //Store the token
                Preferences.Set(Constants.PreferenceKeys.UserApiToken, loginResponse["token"]);
                //Store the role
                Preferences.Set(Constants.PreferenceKeys.UserRole, loginResponse["role"]);
                //Send current notification token
                string currentNotificationToken = await NotificationHandler.Instance.GetTokenAsync();
                await Models.Api.User.UpdateNotificationTokenAsync(loginResponse["token"], currentNotificationToken);
                //Load the page
                await LoadingPopupPage.HideLoadingAsync();
                (App.Current.MainPage as AppShell).SetLoggedUserContextAsync();
            }
            catch(ConnectionException ex)
            {
                //TODO Connection error
                await LoadingPopupPage.HideLoadingAsync();
                await this._messageService.DisplayAlert("Error", "Connexion error.", "Close");
            }
            catch(StatusCodeException ex)
            {
                //Login error
                await LoadingPopupPage.HideLoadingAsync();
                await this._messageService.DisplayAlert("Login error", "Something is wrong in your data.", "Close");
            }
            //IsBusy = false;
        }*/
    }
}
