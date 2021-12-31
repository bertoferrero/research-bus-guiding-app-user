using BusGuiding.Models.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.Views.Dev
{
    public partial class GTFSTestPage : ContentPage
    {

        public GTFSTestPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void StoppedButton_Clicked(object sender, EventArgs e)
        {
            _ = SendSampleAsync("vehicle_stopped");
        }

        private void NextStopButton_Clicked(object sender, EventArgs e)
        {
            _ = SendSampleAsync("vehicle_new_stop");
        }

        private async Task SendSampleAsync(string sampleType)
        {
            try
            {
                StatusLabel.Text = $"Sending {sampleType}";
                await Models.Api.SampleLog.AddSampleLog(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), sampleType, DateTime.UtcNow);
                StatusLabel.Text = $"Sent {sampleType}";
            }
            catch (ConnectionException ex)
            {
                //TODO Connection error
                StatusLabel.Text = $"Connection error {sampleType}";
            }
            catch (StatusCodeException ex)
            {
                //Login error
                StatusLabel.Text = $"Status code error {sampleType}";
            }
        }
    }
}
