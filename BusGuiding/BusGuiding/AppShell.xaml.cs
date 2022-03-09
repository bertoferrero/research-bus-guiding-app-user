
using BusGuiding.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            //Dev
            Routing.RegisterRoute("devdelaytests/fcm", typeof(Views.Dev.FCMTestPage));
            Routing.RegisterRoute("devdelaytests/gtfs", typeof(Views.Dev.GTFSTestPage));
            Routing.RegisterRoute("devdelaytests/internallocation", typeof(Views.Dev.InternalLocationTestPage));
            Routing.RegisterRoute("devfunctionaltests/driver", typeof(Views.Dev.Test2_1__DriverDemoTestPage));
            Routing.RegisterRoute("devfunctionaltests/rider", typeof(Views.Dev.Test2_2__RiderDemoTestPage));
            //Driver
            //Routing.RegisterRoute("driver", typeof(Views.Driver.RouteAndVehicleChoosing));
        }

        public async void SetLoggedUserContextAsync()
        {
            var role = Preferences.Get(Constants.PreferenceKeys.UserRole, "");
            if (role.Equals(Constants.UserRoles.Dev))
            {
                await SetDevContextAsync();
            }
            else if (role.Equals(Constants.UserRoles.Rider))
            {
                await SetRiderContextAsync();
            }
            else if (role.Equals(Constants.UserRoles.Driver))
            {
                await SetDriverContextAsync();
            }
            else
            {
                Preferences.Clear();
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }

        public async Task SetDevContextAsync()
        {
            //FlyoutItems = DevFlyoutItems;
            DevDelaysMenu.FlyoutItemIsVisible = true;
            DevFunctionalDemoMenu.FlyoutItemIsVisible = true;
            //browse to dev main page
            await Shell.Current.GoToAsync("//devdelaytests");
        }

        public async Task SetDriverContextAsync()
        {
            await Shell.Current.GoToAsync("//driver");
        }

        public async Task SetRiderContextAsync()
        {
            await Shell.Current.GoToAsync("//rider");
        }

        private async void LogoutItem_Clicked(object sender, EventArgs e)
        {
            //Clear preferences
            Preferences.Clear();
            //Rediret to login
            await Current.GoToAsync("//login");
        }
    }
}