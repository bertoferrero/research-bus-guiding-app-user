using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.Media;
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


        protected static string driverPriorityChannelId { get => "busguiding_driver_notifications"; }
        protected static string riderPriorityChannelId { get => "busguiding_rider_notifications"; }
        protected static string channelName { get => "Busguiding"; }
        protected static List<string> channelsInitialised = new List<string>();
        int messageId = 0;
        int pendingIntentId = 0;
        NotificationManager notificationManager;

        long[] vibrationPattern = /*{ 100, 200, 300, 600, 500, 400, 300, 200, 400 };*/new long[] { 100, 250, 100, 200 };

       /* Android.Net.Uri notificationUri = Android.Net.Uri.Parse
($"{ContentResolver.SchemeAndroidResource}://{Platform.CurrentActivity.PackageName}/raw/elevator_ding.mp3");*/
            
            //Android.Net.Uri.Parse($"{ContentResolver.SchemeAndroidResource}://com.bertoferrero.busguiding/raw/elevator_ding.mp3");

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
                    notificationManager.DeleteNotificationChannel(channelId);
                    //https://stackoverflow.com/questions/47531742/startforeground-fail-after-upgrade-to-android-8-1
                    var chan = new NotificationChannel(channelId, channelName, NotificationImportance.High);
                    chan.LockscreenVisibility = NotificationVisibility.Private;
                    chan.EnableVibration(true);
                    chan.EnableLights(true);
        
                    //Specific settings
                    if (channelId.Equals(driverPriorityChannelId))
                    {
                        /*var alarmAttributes = new AudioAttributes.Builder()
                                      //.SetContentType(AudioContentType.Sonification)
                                      .SetUsage(AudioUsageKind.Notification)
                                      .Build();
                        chan.SetSound(notificationUri, alarmAttributes);
                        chan.SetVibrationPattern(new long[]{ 100, 200, 300, 600, 500, 400, 300, 200, 400 });*/
                    }
                    else
                    {
                        chan.SetVibrationPattern(vibrationPattern);
                    }
                    //https://docs.microsoft.com/es-es/xamarin/android/app-fundamentals/notifications/local-notifications-walkthrough
                    notificationManager.CreateNotificationChannel(chan);
                }
            }
            return channelId;
        }

        public int ShowNotification(string title, string message, bool forDriver = false)
        {
            string channelId = forDriver ? driverPriorityChannelId:riderPriorityChannelId;
            createNotificationChannel(channelId, channelName);

            Intent intent = new Intent(Platform.CurrentActivity, typeof(MainActivity));
            intent.PutExtra("title", title);
            intent.PutExtra("message", message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(Platform.CurrentActivity, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            Notification.Builder builder = new Notification.Builder(Platform.CurrentActivity, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetStyle(new Notification.BigTextStyle().BigText(message))
                //.SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamagonBlue))
                .SetSmallIcon(Resource.Mipmap.icon);
            //.SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);
            if (forDriver)
            {

            }
            else
            {
                //builder.SetVibrate(vibrationPattern);
            }

            Notification notification = builder.Build();
            notificationManager.Notify(messageId++, notification);
            return messageId;

            //Cerrar notificación
            //https://stackoverflow.com/questions/6533360/close-android-notification
            //Sonidos y vibraciones, pero tiene que ir en el canal. Crear en la interfaz dos métodos (si la interfaz lo permite) que sea ShowRiderNotification y ShowDriverNotification, que inicialicen el canal si no lo está y envien la notificación
            //https://stackoverflow.com/questions/18253482/vibrate-and-sound-defaults-on-notification
        }
        #endregion
    }
}