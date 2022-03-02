using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BusGuiding.DependencyServices;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BusGuiding.Droid.DependencyService.DeviceTaskManager))]
namespace BusGuiding.Droid.DependencyService
{
    class DeviceTaskManager : IDeviceTaskManager
    {
        public void BringToForeground()
        {
            Intent intent = new Intent(Platform.CurrentActivity, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            Platform.CurrentActivity.StartActivity(intent);
        }
    }
}