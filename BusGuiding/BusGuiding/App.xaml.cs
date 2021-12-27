using BusGuiding.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusGuiding
{
    public partial class App : Application
    {

        public App()
        {
            DependencyService.Register<ViewModels.Services.IMessageService, Views.Services.MessageService>();

            InitializeComponent();

            MainPage = new AppShell();

            ////Inicializamos elementos
            //Notificaciones - Basado en https://stackoverflow.com/questions/30213726/pass-data-from-android-service-to-contentpage-in-xamarin-form-based-application
            var notificationHandler = NotificationHandler.Instance;
            notificationHandler.TokenUpdated += NotificationHandler_TokenUpdated;

        }

        private void NotificationHandler_TokenUpdated(object sender, string newToken)
        {
            //get user token, if it is defined, send the new notification token through api connection
            var apiUserToken = Preferences.Get(Constants.PreferenceKeys.UserApiToken,"");
            if (!string.IsNullOrEmpty(apiUserToken))
            {
                try {
                    _ = Models.Api.User.UpdateNotificationTokenAsync(apiUserToken, newToken);
                } catch (Exception) { }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
