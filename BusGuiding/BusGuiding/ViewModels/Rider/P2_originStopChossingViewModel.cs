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
    public class P2_originStopChossingViewModel : BaseViewModel
    {

        private string stopCodeId = "";
        public Command ReadFromStopCodeFieldCommand { get; }
        public Command GeolocaliseCommand { get; }

        private readonly Services.IMessageService _messageService;

        public string StopCodeId
        {
            get
            {
                return stopCodeId;
            }
            set
            {
                SetProperty(ref stopCodeId, value);
            }
        }

        public P2_originStopChossingViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            ReadFromStopCodeFieldCommand = new Command(OnReadFromStopCodeFieldCommandAsync);
            GeolocaliseCommand = new Command(OnGeolocaliseCommand);
            resetView();
        }

        public void resetView()
        {
            StopCodeId = "";
        }

        public async void OnReadFromStopCodeFieldCommandAsync()
        {
            string stopCodeValue = StopCodeId.Trim();
            if (string.IsNullOrEmpty(stopCodeValue))
            {
                return;
            }
            //Request by the stop information
            await LoadingPopupPage.ShowLoading();
            try
            {
                var stopInformation = await Models.Api.Stop.GetOneByStopCode(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), stopCodeValue);
            }
            catch (ConnectionException ex)
            {
                await this._messageService.DisplayAlert("Error", "Connexion error.", "Close");
                return;
            }
            catch (StatusCodeException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this._messageService.DisplayAlert("Error", "Stop not found.", "Close");
                }
                else
                {
                    await this._messageService.DisplayAlert("Error", "Connexion error.", "Close");
                }
            }
            finally
            {
                await LoadingPopupPage.HideLoadingAsync();
            }

            //TODO pasamos a la siguiente pagina
            int a = 3;
        }
        public async void OnGeolocaliseCommand()
        {
            await LoadingPopupPage.ShowLoading();
            //Get Geolocation
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request);
            if (location == null)
            {
                await this._messageService.DisplayAlert("Error", "Geolocation data cannot be got. Please try again.", "Close");
                return;

            }
            //Request for the nearest stop
            var latitude = location.Latitude;
            var longitude = location.Longitude;
            try
            {
                var stopInformation = await Models.Api.Stop.GetNearest(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), latitude, longitude);
            }
            catch (Exception ex)
            {
                await this._messageService.DisplayAlert("Error", "Connexion error.", "Close");
                return;
            }
            finally
            {
                await LoadingPopupPage.HideLoadingAsync();
            }

            //TODO load next page

        }

    }
}
