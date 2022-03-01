using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BusGuiding.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace BusGuiding.Droid.Services
{
    [Service]
    class GPSSenderService : Service
    {
        static readonly string TAG = typeof(GPSSenderService).FullName;

        bool isStarted;
        Handler handler;
        Action runnable;

        public override void OnCreate()
        {
            base.OnCreate();

            _ = Log.Info(TAG, "OnCreate: the service is initializing.");
            handler = new Handler(Looper.MainLooper);

            // This Action is only for demonstration purposes.
            runnable = new Action(async () =>
            {
                Log.Info(TAG, "RUN");
                await GeoData.SendGeoDataToServerAsync();
                /*if (timestamper == null)
                {
                    Log.Wtf(TAG, "Why isn't there a Timestamper initialized?");
                }
                else
                {
                    string msg = timestamper.GetFormattedTimestamp();
                    Log.Debug(TAG, msg);
                    Intent i = new Intent(Constants.NOTIFICATION_BROADCAST_ACTION);
                    i.PutExtra(Constants.BROADCAST_MESSAGE_KEY, msg);
                    Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).SendBroadcast(i);
                    handler.PostDelayed(runnable, Constants.DELAY_BETWEEN_LOG_MESSAGES);
                }*/
                if (isStarted)
                {
                    handler.PostDelayed(runnable, 1000); //TODO configuración
                }
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals("GPSSenderService.action.START_SERVICE")) //TODO poner en algun lugar localizado
            {
                if (isStarted)
                {
                    Log.Info(TAG, "OnStartCommand: The service is already running.");
                }
                else
                {
                    Log.Info(TAG, "OnStartCommand: The service is starting.");
                    RegisterForegroundService();
                    handler.PostDelayed(runnable, 1000); //TODO configuración
                    isStarted = true;
                }
            }
            else if (intent.Action.Equals("GPSSenderService.action.STOP_SERVICE"))
            {
                Log.Info(TAG, "OnStartCommand: The service is stopping.");
                StopForeground(true);
                StopSelf();
                isStarted = false;

            }
            else if (intent.Action.Equals("GPSSenderService.action.RESTART_SERVICE"))
            {
                Log.Info(TAG, "OnStartCommand: Restarting the timer.");
            }

            // This tells Android not to restart the service if it is killed to reclaim resources.
            //https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/services/creating-a-service/started-services
            return StartCommandResult.Sticky;
        }


        public override IBinder OnBind(Intent intent)
        {
            // Return null because this is a pure started service. A hybrid service would return a binder that would
            // allow access to the GetFormattedStamp() method.
            return null;
        }


        public override void OnDestroy()
        {
            
            Log.Info(TAG, "OnDestroy: The started service is shutting down.");

            // Stop the handler.
            handler.RemoveCallbacks(runnable);

            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(10000); //TODO configuración

            isStarted = false;
            base.OnDestroy();
        }

        void RegisterForegroundService()
        {
            createNotificationChannel("my_notification_channel", "BusGuiding");
            var notification = new Notification.Builder(this, "my_notification_channel") //TODO configuracion
                .SetContentTitle("BusGuiding")
                .SetContentText("Sending GPS location")
                //.SetSmallIcon(Resource.Drawable.ic_stat_name)
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                //.AddAction(BuildRestartTimerAction())
                //.AddAction(BuildStopServiceAction())
                .Build();

            int SERVICE_RUNNING_NOTIFICATION_ID = 10000; //TODO config
            // Enlist this instance of the service as a foreground service
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }

        private string createNotificationChannel(string channelId, string channelName)
        {
            //https://stackoverflow.com/questions/47531742/startforeground-fail-after-upgrade-to-android-8-1
            var chan = new NotificationChannel(channelId, channelName, NotificationImportance.None);
            chan.LockscreenVisibility = NotificationVisibility.Private;
            //https://docs.microsoft.com/es-es/xamarin/android/app-fundamentals/notifications/local-notifications-walkthrough
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(chan);
            return channelId;
        }

        /// <summary>
        /// Builds a PendingIntent that will display the main activity of the app. This is used when the 
        /// user taps on the notification; it will take them to the main activity of the app.
        /// </summary>
        /// <returns>The content intent.</returns>
        PendingIntent BuildIntentToShowMainActivity()
        {
            //TODO al pulsar en la notificacion tiene que volver a la aplicacion
            var notificationIntent = new Intent(this, typeof(MainActivity));
            //notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            //notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return pendingIntent;
        }
    }
}