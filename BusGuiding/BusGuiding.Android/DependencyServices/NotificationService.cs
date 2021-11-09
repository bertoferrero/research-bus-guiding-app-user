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
using Xamarin.Forms;

[assembly: Dependency(typeof(BusGuiding.Droid.DependencyService.NotificationServices))]
namespace BusGuiding.Droid.DependencyService
{
    class NotificationServices: INotificationService
    {
        public async Task<string> GetDeviceTokenAsync()
        {
            var token = await FirebaseMessaging.Instance.GetToken();
            return token.ToString();
        }
    }
}