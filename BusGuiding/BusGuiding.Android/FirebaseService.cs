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
            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "push.newtoken", p0);
        }

        public override void OnMessageReceived(RemoteMessage p0)
        {
            base.OnMessageReceived(p0);
            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "push.newnotification", p0.Data);
        }
    }
}