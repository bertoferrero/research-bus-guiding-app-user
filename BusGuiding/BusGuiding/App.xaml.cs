using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusGuiding
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

        //Utilizar mejor esta solución
        https://stackoverflow.com/questions/30213726/pass-data-from-android-service-to-contentpage-in-xamarin-form-based-application

            //Inicializamos los eventos de notificación
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "push.newtoken", async (sender, arg) =>
             {
                 NotificationHandler.OnNewToken(arg);
                 string token = await NotificationHandler.GetTokenAsync();
             });
            MessagingCenter.Subscribe<App, Dictionary<string,string>>((App)Application.Current, "push.newnotification", (sender, arg) =>
            {
                NotificationHandler.OnNewNotification(arg);
            });

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
