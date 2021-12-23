using BusGuiding.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusGuiding
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            ////Inicializamos elementos
            //Notificaciones - Basado en https://stackoverflow.com/questions/30213726/pass-data-from-android-service-to-contentpage-in-xamarin-form-based-application
            NotificationHandler.initEvents();

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
