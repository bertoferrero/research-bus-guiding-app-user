using BusGuiding.DependencyServices;
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
            int a = 3;
            /*if (e["notification_type"] != "StopRequest")
            {
                return;
            }
            var extraData = $"vehicle_id: {e["vehicle_id"]}, line_id: {e["line_id"]}, status: {e["status"]}, stop_id:{e["stop_id"]}";
            GeneralLog.Text = "Notificacion de parada recibida";
            _ = SendSampleAsync("t2.1_notification_received", extraData);*/
        }
    }
}
