using Acr.UserDialogs;
using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver
{

    [QueryProperty(nameof(StopCode), "stop_code")]
    [QueryProperty(nameof(StopName), "stop_name")]
    [QueryProperty(nameof(StopSchemaId), "stop_schema_id")]
    [QueryProperty(nameof(RouteSchemaId), "route_schema_id")]
    [QueryProperty(nameof(RouteName), "route_name")]
    [QueryProperty(nameof(RouteColor), "route_color")]
    public class P4_DestinationStopChoosingViewModel : BaseViewModel
    {
        private readonly Services.IMessageService _messageService;
        protected string stopCode = "";
        public string StopCode
        {
            get
            {
                return stopCode;
            }
            set
            {
                SetProperty(ref stopCode, value);
            }
        }

        protected string stopName = "";
        public string StopName
        {
            get
            {
                return stopName;
            }
            set
            {
                SetProperty(ref stopName, value);
            }
        }
        protected string stopSchemaId = "";
        public string StopSchemaId
        {
            get
            {
                return stopSchemaId;
            }
            set
            {
                SetProperty(ref stopSchemaId, value);
            }
        }

        protected string routeSchemaId = "";
        public string RouteSchemaId
        {
            get
            {
                return routeSchemaId;
            }
            set
            {
                string originalValue = routeSchemaId;
                SetProperty(ref routeSchemaId, value);
                if (!originalValue.Equals(routeSchemaId))
                {
                    loadStopListAsync();
                }
            }
        }

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

        protected StopTmp selectedStop = null;
        public StopTmp SelectedStop
        {
            get
            {
                return selectedStop;
            }
            set
            {
                SetProperty(ref selectedStop, value);
                onStopSelected();
            }
        }

        protected ObservableCollection<StopTmp> stops = new ObservableCollection<StopTmp>();
        public ObservableCollection<StopTmp> Stops
        {
            get
            {
                return stops;
            }
            set
            {
                SetProperty(ref stops, value);
            }
        }




        public P4_DestinationStopChoosingViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            resetView();
        }

        public void resetView()
        {
        }

        protected async void loadStopListAsync()
        {
            await LoadingPopupPage.ShowLoading();
            try
            {
                var serverStops = await Models.Api.Route.GetOne(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), RouteSchemaId);
                //Separate head list from tail. We are wanting to display stops from the origin one
                ObservableCollection<StopTmp> newStops = new ObservableCollection<StopTmp>();
                ObservableCollection<StopTmp> newStopsTail = new ObservableCollection<StopTmp>();
                bool useTail = true;
                foreach (Dictionary<string, string> serverStop in serverStops)
                {
                    var stop = new StopTmp()
                    {
                        SchemaId = serverStop["id"],
                        Name = serverStop["name"],
                        Code = serverStop["code"]
                    };
                    if (useTail)
                    {
                        newStopsTail.Add(stop);
                    }
                    else
                    {
                        newStops.Add(stop);
                    }

                    if(stop.SchemaId == StopSchemaId)
                    {
                        useTail = false;
                    }
                }
                Stops = new ObservableCollection<StopTmp>(newStops.Concat(newStopsTail));
            }
            catch (Exception ex)
            {
                await this._messageService.DisplayAlert("Error", "Routes cannot be got, please, check your internet connection.", "Close");
                await Shell.Current.GoToAsync("..");
                return;
            }
            finally
            {
                await LoadingPopupPage.HideLoadingAsync();
            }
        }

        protected async void onStopSelected()
        {
            if (SelectedStop != null)
            {
                bool answer = await UserDialogs.Instance.ConfirmAsync($"Do you want to travel from {StopName} to {SelectedStop.Name} by using the line {RouteName}?", "Confirmation");
                if (answer)
                {
                    await Shell.Current.GoToAsync($"p5?origin_stop_code={stopCode}&origin_stop_name={stopName}&origin_stop_schema_id={stopSchemaId}&route_schema_id={RouteSchemaId}&route_name={RouteName}&route_color={RouteColor}&destination_stop_code={SelectedStop.Code}&destination_stop_name={SelectedStop.Name}&destination_stop_schema_id={SelectedStop.SchemaId}");
                }
            }
        }

    }

    public class StopTmp
    {
        public string SchemaId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

}
