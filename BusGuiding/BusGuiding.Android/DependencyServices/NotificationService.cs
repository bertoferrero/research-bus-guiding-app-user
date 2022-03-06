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

[assembly: Dependency(typeof(BusGuiding.Droid.DependencyService.NotificationServices))]
namespace BusGuiding.Droid.DependencyService
{
    class NotificationServices: INotificationService
    {
        public static NotificationServices mySelf;
        public event EventHandler<string> tokenUpdated;
        public event EventHandler<IDictionary<string, string>> pushReceived;


        protected static string highPriorityChannelId { get => "busguiding_high_notifications"; }
        protected static string lowPriorityChannelId { get => "busguiding_notifications"; }
        protected static string channelName { get => "Busguiding"; }
        protected static List<string> channelsInitialised = new List<string>();
        int messageId = 0;
        int pendingIntentId = 0;
        NotificationManager notificationManager;

        public void start()
        {
            mySelf = this;
            notificationManager = (NotificationManager)(Platform.CurrentActivity.GetSystemService(Context.NotificationService));
        }

        #region Recepción de tokens y notificaciones

        public async Task<string> GetDeviceTokenAsync()
        {
            var token = await FirebaseMessaging.Instance.GetToken();
            return token.ToString();
        }

        public void receivedNewToken(string token)
        {
            tokenUpdated(this, token);
        }

        public void receivedNewNotification(IDictionary<string, string> data)
        {
            pushReceived(this, data);
        }
        #endregion

        #region Mostrar notificaciones
        private string createNotificationChannel(string channelId, string channelName)
        {
            if (!channelsInitialised.Contains(channelId))
            {
                channelsInitialised.Add(channelId);
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    //https://stackoverflow.com/questions/47531742/startforeground-fail-after-upgrade-to-android-8-1
                    var chan = new NotificationChannel(channelId, channelName, NotificationImportance.High);
                    chan.LockscreenVisibility = NotificationVisibility.Private;
                    chan.EnableVibration(true);
                    chan.EnableLights(true);
                    //https://docs.microsoft.com/es-es/xamarin/android/app-fundamentals/notifications/local-notifications-walkthrough
                    notificationManager.CreateNotificationChannel(chan);
                }
            }
            return channelId;
        }

        public int ShowNotification(string title, string message, bool highImportance = false)
        {
            string channelId = highImportance?highPriorityChannelId:lowPriorityChannelId;
            createNotificationChannel(channelId, channelName);

            Intent intent = new Intent(Platform.CurrentActivity, typeof(MainActivity));
            intent.PutExtra("title", title);
            intent.PutExtra("message", message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(Platform.CurrentActivity, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            Notification.Builder builder = new Notification.Builder(Platform.CurrentActivity, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                //.SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamagonBlue))
                .SetSmallIcon(Resource.Mipmap.icon);
                //.SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            notificationManager.Notify(messageId++, notification);
            return messageId;

        //Cerrar notificación
        //https://stackoverflow.com/questions/6533360/close-android-notification
        }
        #endregion
    }
}