using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using Xamarin.Forms;

namespace BusGuiding.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            BusGuiding.Droid.DependencyService.NotificationServices.mySelf.receivedNewToken(p0);
        }

        public override void OnMessageReceived(RemoteMessage p0)
        {
            base.OnMessageReceived(p0);
            BusGuiding.Droid.DependencyService.NotificationServices.mySelf.receivedNewNotification(p0.Data);
        }
    }
}