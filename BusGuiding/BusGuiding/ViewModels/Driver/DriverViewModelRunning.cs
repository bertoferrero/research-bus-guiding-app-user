using BusGuiding.DependencyServices;
using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Driver;
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
        private bool showRunningForm = false;
        public Command FinishCommand { get; }
        private bool sendGpsSwitchToggle = false;

        public bool ShowRunningForm
        {
            get => showRunningForm;
            set
            {
                if (value)
                {
                    startRunning();
                }
                else
                {
                    stopRunningAsync();
                }
                SetProperty(ref showRunningForm, value);
            }
        }

        public bool SendGpsSwitchToggle
        {
            get => sendGpsSwitchToggle;
            set
            {
                setGpsService(value);
                SetProperty(ref sendGpsSwitchToggle, value);
            }
        }

        partial void runningFinalize()
        {
            setGpsService(false);
            NotificationHandler.Instance.NewNotification -= NotificationHandler_NewNotification;
            _ = StopAlertPopupPage.CloseStopAlertAsync();
        }

        public void OnFinishClicked()
        {
            IsRunning = false;
        }

        private void startRunning()
        {
            //Inicializamos las notificaciones
            NotificationHandler.Instance.NewNotification += NotificationHandler_NewNotification;
        }

        private async void stopRunningAsync()
        {
            //ponemos el waiting
            await LoadingPopupPage.ShowLoading();
            //Cerramos ventanas de alerta
            _ = StopAlertPopupPage.CloseStopAlertAsync();
            //Paramos listener de eventos
            NotificationHandler.Instance.NewNotification -= NotificationHandler_NewNotification;
            //Paramos listener del gps
            SendGpsSwitchToggle = false;
            //https://github.com/xamarin/monodroid-samples/tree/main/ApplicationFundamentals/ServiceSamples/ForegroundServiceDemo
            //desenlazamos conductor y vehículo
            try
            {
                await Models.Api.User.UpdateDriverVehicleAndRoute(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), "", "");
            }
            catch (Exception ex)
            {
            }
            await LoadingPopupPage.HideLoadingAsync();
        }

        private void setGpsService(bool active)
        {
            ILocationSenderService currentService = DependencyService.Get<ILocationSenderService>();
            if (active)
            {
                currentService.Start();
            }
            else
            {
                currentService.Stop();
            }
        }


        private void NotificationHandler_NewNotification(object sender, IDictionary<string, string> e)
        {
            //Traer aplicacion a primer plano
            //https://stackoverflow.com/questions/51393431/is-there-a-way-to-bring-an-application-to-foreground-on-push-notification-receiv
            if (e["notification_type"] == "StopRequest")
            {
                //Show the notification
                //NotificationHandler.Instance.ShowNotification("STOP!", $"Stop is required for stop number {e["stop_id"]}", true);
                //Bring the app to the front
                DependencyService.Get<IDeviceTaskManager>().BringToForeground();
                //Show the popup
                _ = StopAlertPopupPage.ShowStopAlertAsync(e["stop_id"]);
            }
            else if(e["notification_type"] == "DismissStopRequest")
            {
                //Close the notification
                _ = StopAlertPopupPage.CloseStopAlertAsync();
            }
        }

        
    }
}
