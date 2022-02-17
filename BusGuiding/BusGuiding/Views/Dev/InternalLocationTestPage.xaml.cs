using BusGuiding.Models.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading;

namespace BusGuiding.Views.Dev
{
    public partial class InternalLocationTestPage : ContentPage
    {
        private bool testRunning = false;
        private System.Timers.Timer gpsTimer = null;
        CancellationTokenSource gpsCancellationToken = null;

        public InternalLocationTestPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _ = StopTestAsync();
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            if (testRunning)
            {
                _ = StopTestAsync();
            }
            else
            {
                _ = StartTestAsync();
            }
        }

        private async Task StartTestAsync()
        {
            string routeId = RouteIdEntry.Text.Trim();
            string vehicleId = VehicleIdEntry.Text.Trim();
            if(String.IsNullOrEmpty(routeId) || String.IsNullOrEmpty(vehicleId))
            {
                SentStatusLabel.Text = "Route and Vehicle fields must be filled";
                return;
            }

            //Notes for GPS
            //https://stackoverflow.com/questions/45007614/alternative-to-thread-sleep-in-a-while-loop
            //https://github.com/shernandezp/XamarinForms.LocationService

            StartButton.IsEnabled = false;
            //Send route and vehicle ID as update for this user
            if(!await SendVehicleAndRouteAsync(vehicleId, routeId))
            {
                StartButton.IsEnabled = true;
                return;
            }
            StartButton.IsEnabled = true;

            testRunning = true;

            //Start the GPS subroutine
            startGPSTimer();

            //set label and button texts
            RouteIdEntry.IsEnabled = VehicleIdEntry.IsEnabled = false;
            StoppedButton.IsVisible = NextStopButton.IsVisible = true;
            SentStatusLabel.Text = "Running";
            StartButton.Text = "Stop";
        }

        private async Task StopTestAsync()
        {
            //Stop GPS subroutine
            stopGPSTimer();

            //Send emptu route and vehicle id
            await SendVehicleAndRouteAsync("", "");
            //set label and button texts
            RouteIdEntry.IsEnabled = VehicleIdEntry.IsEnabled = true;
            StoppedButton.IsVisible = NextStopButton.IsVisible = false;
            SentStatusLabel.Text = "Stopped";
            StartButton.Text = "Start";

            testRunning = false;
        }



        private void StoppedButton_Clicked(object sender, EventArgs e)
        {
            _ = SendSampleAsync("vehicle_stopped");
        }

        private void NextStopButton_Clicked(object sender, EventArgs e)
        {
            _ = SendSampleAsync("vehicle_new_stop");
        }

        private void startGPSTimer() {
            if (testRunning && gpsTimer == null)
            {
                gpsTimer = new System.Timers.Timer(1000);
                gpsTimer.Elapsed += async (sender, args) => {
                    await sendGeolocationData();
                    if (testRunning && gpsTimer != null)
                    {
                        gpsTimer.Start();
                    }
                };
                gpsTimer.Enabled = true;
                gpsTimer.AutoReset = false;
                gpsTimer.Start();
            }
        }

        private void stopGPSTimer()
        {
            if(gpsTimer != null)
            {
                gpsTimer.Stop();
                gpsTimer = null;
            }
            if (gpsCancellationToken != null)
            {
                gpsCancellationToken.Cancel();
                gpsCancellationToken = null;
            }
        }

        private async Task sendGeolocationData()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10000));
                gpsCancellationToken = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, gpsCancellationToken.Token);
                gpsCancellationToken = null;
                var latitude = location.Latitude;
                var longitude = location.Longitude;
                _ = Models.Api.User.UpdateDriverLatitudeLongitude(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), latitude, longitude);
                _ = SendSampleAsync("geolocation_update", location.ToString());
                GPSStatusLabel.Text = $"Sent GPS {latitude} {longitude}";
            }
            catch(Exception ex){
                GPSStatusLabel.Text = "Exception happend getting or sending geolocation: " + ex.Message;
            }
        }

        private async Task SendSampleAsync(string sampleType, string extraData = "")
        {
            try
            {
                SentStatusLabel.Text = $"Sending {sampleType}";
                await Models.Api.SampleLog.AddSampleLog(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), sampleType, DateTime.UtcNow, extraData);
                SentStatusLabel.Text = $"Sent {sampleType}";
            }
            catch (ConnectionException ex)
            {
                //TODO Connection error
                SentStatusLabel.Text = $"Connection error {sampleType}";
            }
            catch (StatusCodeException ex)
            {
                //Login error
                SentStatusLabel.Text = $"Status code error {sampleType}";
            }
        }

        private async Task<bool> SendVehicleAndRouteAsync(string vehicleId, string routeId)
        {
            try
            {
                SentStatusLabel.Text = $"Sending vehicle and route";
                Dictionary<string,string> result = await Models.Api.User.UpdateDriverVehicleAndRoute(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), vehicleId, routeId);
                SentStatusLabel.Text = $"Sent vehicle and route";
                return true;
            }
            catch (ConnectionException ex)
            {
                //TODO Connection error
                SentStatusLabel.Text = $"Connection error "+ex.Message;
                return false;
            }
            catch (StatusCodeException ex)
            {
                //Login error
                SentStatusLabel.Text = $"Status code error " + ex.Message;
                return false;
            }
        }
    }
}
