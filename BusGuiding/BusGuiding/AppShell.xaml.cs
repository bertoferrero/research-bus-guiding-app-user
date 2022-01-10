
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
            Routing.RegisterRoute("devdelaytests/fcm", typeof(Views.Dev.FCMTestPage));
            Routing.RegisterRoute("devdelaytests/gtfs", typeof(Views.Dev.GTFSTestPage));
            Routing.RegisterRoute("devfunctionaltests/driver", typeof(Views.Dev.DriverDemoTestPage));
            //Routing.RegisterRoute("devfunctionaltests/rider", typeof(Views.Dev.GTFSTestPage));
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

            }
            else if (role.Equals(Constants.UserRoles.Driver))
            {

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

    }
}