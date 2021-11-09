using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;

namespace BusGuiding.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            Log.Debug("push", "Refreshed token: " + p0);
            // can send token to backend server or store in secure storage
        }

        public override void OnMessageReceived(RemoteMessage p0)
        {
            base.OnMessageReceived(p0);
            Log.Debug("push", "Message received: " + p0.GetNotification().Body);
        }
    }
}