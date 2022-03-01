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
        }

        public void OnFinishClicked()
        {
            IsRunning = false;
        }

        private async void startRunning()
        {
        }

        private async void stopRunningAsync()
        {
            //ponemos el waiting
            await LoadingPopupPage.ShowLoading();
            //TODO Paramos listener de eventos
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
    }
}
