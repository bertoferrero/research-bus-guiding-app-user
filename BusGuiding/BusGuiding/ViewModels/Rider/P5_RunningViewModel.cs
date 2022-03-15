using Acr.UserDialogs;
using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver
{

    [QueryProperty(nameof(OriginStopCode), "origin_stop_code")]
    [QueryProperty(nameof(OriginStopName), "origin_stop_name")]
    [QueryProperty(nameof(OriginStopSchemaId), "origin_stop_schema_id")]
    [QueryProperty(nameof(RouteSchemaId), "route_schema_id")]
    [QueryProperty(nameof(RouteName), "route_name")]
    [QueryProperty(nameof(RouteColor), "route_color")]
    [QueryProperty(nameof(DestinationStopCode), "destination_stop_code")]
    [QueryProperty(nameof(DestinationStopName), "destination_stop_name")]
    [QueryProperty(nameof(DestinationStopSchemaId), "destination_stop_schema_id")]
    //[QueryProperty(nameof(Subroute), "subroute")]
    public class P5_RunningViewModel : BaseViewModel, IQueryAttributable, INotifyPropertyChanged
    {
        private readonly Services.IMessageService _messageService;
        protected string currentStatus = "";
        public string CurrentStatus
        {
            get
            {
                return currentStatus;
            }
            set
            {
                SetProperty(ref currentStatus, value);
            }
        }
        public string OriginStopCode { get; private set; }

        protected string originStopName = "";
        public string OriginStopName
        {
            get
            {
                return originStopName;
            }
            set
            {
                SetProperty(ref originStopName, value);
            }
        }
        public string OriginStopSchemaId { get; private set; }

        public string RouteSchemaId { get; private set; }

        protected string routeName = "";
        public string RouteName
        {
            get
            {
                return routeName;
            }
            set
            {
                SetProperty(ref routeName, value);
            }
        }

        protected string routeColor = "";
        public string RouteColor
        {
            get
            {
                return routeColor;
            }
            set
            {
                SetProperty(ref routeColor, value);
            }
        }

        public string DestinationStopCode { get; private set; }

        protected string destinationStopName = "";
        public string DestinationStopName
        {
            get
            {
                return destinationStopName;
            }
            set
            {
                SetProperty(ref destinationStopName, value);
            }
        }
        protected string DestinationStopSchemaId { get; private set; }

        public List<Dictionary<string, string>> Subroute { get; private set; }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            /*string name = HttpUtility.UrlDecode(query["name"]);
            string location = HttpUtility.UrlDecode(query["location"]);
            ...     */
            //TODO asignar todas las propiedades y llamar a init fase 1
            //TODO serializar subroute como json para pasarlo y recibirlo
            //TODO hacer P5 un partial, creando una clase para los parámetros

            int a = 4;
        }



        public P5_RunningViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            dismissRequests();
            int a = 3;
        }

        protected async void dismissRequests()
        {
            //Stops
            var taskInvalidateStops = Models.Api.StopRequest.InvalidatePendentRequestsAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""));
            //Notifications
            var taskInvalidateNotifications = Models.Api.NotificationTopics.ClearNotificationTokenListAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""));

            await taskInvalidateStops;
            await taskInvalidateNotifications;
        }  

        protected async void initPhase1()
        {
            await LoadingPopupPage.ShowLoading();
            try
            {
                
            }catch(Exception ex)
            {
                //TODO mostrar error y echar atrás al cerrarse
            }
            finally
            {
                await LoadingPopupPage.HideLoadingAsync();
            }
        }
        


    }

}
