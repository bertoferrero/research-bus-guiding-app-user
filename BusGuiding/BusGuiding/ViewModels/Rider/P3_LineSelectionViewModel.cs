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
    public class P3_LineSelectionViewModel : BaseViewModel
    {
        private readonly Services.IMessageService _messageService;
        protected string stopCode = "";
        public Command ContinueCommand { get; }
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
                string originalValue = stopSchemaId;
                SetProperty(ref stopSchemaId, value);
                if (!originalValue.Equals(stopSchemaId))
                {
                    _ = loadRouteListAsync();
                }
            }
        }

        protected RouteTmp selectedRoute = null;
        public RouteTmp SelectedRoute
        {
            get
            {
                return selectedRoute;
            }
            set
            {
                SetProperty(ref selectedRoute, value);
                OnContinueCommand();
            }
        }

        protected ObservableCollection<RouteTmp> routes = new ObservableCollection<RouteTmp>();
        public ObservableCollection<RouteTmp> Routes
        {
            get
            {
                return routes;
            }
            set
            {
                SetProperty(ref routes, value);
            }
        }


        public P3_LineSelectionViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            ContinueCommand = new Command(OnContinueCommand);
            resetView();
        }

        public void resetView()
        {
            SelectedRoute = null;
        }

        protected async Task loadRouteListAsync()
        {
            await LoadingPopupPage.ShowLoading();
            try
            {
                var serverRoutes = await Models.Api.Stop.GetRoutes(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), StopSchemaId);
                ObservableCollection<RouteTmp> newRoutes = new ObservableCollection<RouteTmp>();
                foreach(Dictionary<string,string> serverRoute in serverRoutes)
                {
                    newRoutes.Add(new RouteTmp()
                    {
                        Color = serverRoute["color"],
                        Name = serverRoute["name"],
                        SchemaId = serverRoute["id"]
                    });
                }
                Routes = newRoutes;
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
        public async void OnContinueCommand()
        {
            if(SelectedRoute != null)
            {
                await Shell.Current.GoToAsync($"p4?stop_code={stopCode}&stop_name={stopName}&stop_schema_id={stopSchemaId}&route_schema_id={SelectedRoute.SchemaId}&route_name={SelectedRoute.Name}&route_color={SelectedRoute.Color}");
                SelectedRoute = null;
            }
        }

    }


    public class RouteTmp
    {
        public string SchemaId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
