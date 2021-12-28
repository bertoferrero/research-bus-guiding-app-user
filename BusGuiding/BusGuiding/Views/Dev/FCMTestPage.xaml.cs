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
    public partial class FCMTestPage : ContentPage
    {
        private bool testRunning = false;

        public FCMTestPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _ = this.StopFCMTestsAsync();
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            if (testRunning)
            {
                _ = StopFCMTestsAsync();
            }
            else
            {
                _ = StartFCMTestsAsync();
            }
        }

        private async Task StartFCMTestsAsync()
        {
            //set label and button texts
            StatusLabel.Text = "Running";
            StartButton.Text = "Stop";
            //register events of NotificanHandler
            NotificationHandler.Instance.NewNotification += NotificationHandler_NewNotification;
            //send topic registration to server
            await Models.Api.NotificationTopics.SubscribeNotificationTokenListAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), new List<string>() { "vehicle.0.in_transit_to.0" });
            testRunning = true;
        }

        private async Task StopFCMTestsAsync()
        {
            //send remove topic registration to server
            await Models.Api.NotificationTopics.ClearNotificationTokenListAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""));
            //unregister events from NotificationHandler
            NotificationHandler.Instance.NewNotification -= NotificationHandler_NewNotification;
            //set label and button texts
            StatusLabel.Text = "Stopped";
            StartButton.Text = "Start";

            testRunning = false;
        }

        private void NotificationHandler_NewNotification(object sender, IDictionary<string, string> e)
        {
            int a = 3;
        }
    }
}
