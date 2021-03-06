using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BusGuiding.DependencyServices;
using BusGuiding.Droid.Services;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BusGuiding.Droid.DependencyService.LocationSenderService))]
namespace BusGuiding.Droid.DependencyService
{
    class LocationSenderService : ILocationSenderService
    {
        private static bool initialised = false; 
        private static Intent startServiceIntent = null;
        private static Intent stopServiceIntent = null;
        private static bool isLaunched = false;


        public void Start()
        {
            initialise();
            if (startServiceIntent != null)
            {
                if (isLaunched)
                {
                    Stop();
                }

                Platform.CurrentActivity.StartForegroundService(startServiceIntent);
                isLaunched = true;
            }
        }

        public void Stop()
        {
            if (initialised && stopServiceIntent != null && isLaunched)
            {
                Platform.CurrentActivity.StopService(stopServiceIntent);
                isLaunched = false;
            }
        }

        private void initialise()
        {
            if (!initialised)
            {
                startServiceIntent = new Intent(Platform.CurrentActivity, typeof(GPSSenderService));
                startServiceIntent.SetAction("GPSSenderService.action.START_SERVICE");

                stopServiceIntent = new Intent(Platform.CurrentActivity, typeof(GPSSenderService));
                stopServiceIntent.SetAction("GPSSenderService.action.STOP_SERVICE");

                initialised = true;
            }
        }
    }
}