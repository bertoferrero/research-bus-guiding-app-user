using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.Views.Dev
{
    public partial class DriverDemoTestPage : ContentPage
    {
        private bool testRunning = false;
        private List<Dictionary<string, string>> stopsList = null;

        public DriverDemoTestPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //TODO ocultar panel de trabajo
            //TODO mostrar formulario
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //TODO desubscripcion de eventos
            //TODO desvinculado de usuario y vehiculo
            //TODO desubscripción de eventos locales
            //TODO limpieza de formulario
            //TODO ocultar panel de trabajo
        }

        //Al pinchar en comenzar
        //TODO descargar lista de paradas de la linea, localizar la actual
        //TODO enviar vinculación entre usuario y vehículo
        //Localizar la parada siguiente a la actual y enviar solicitud de parada

        //Al pulsar en Stop
        //


        private async void StartButton_ClickedAsync(object sender, EventArgs e)
        {
            string routeId = RouteIdEntry.Text.Trim();
            string vehicleId = VehicleIdEntry.Text.Trim();
            string lastStop = LastStopId.Text.Trim();
            if (String.IsNullOrEmpty(routeId) || String.IsNullOrEmpty(vehicleId) || String.IsNullOrEmpty(lastStop))
            {
                GeneralLog.Text = "Route, Stop and Vehicle fields must be filled";
                return;
            }
            GeneralLog.Text = "";

            //Get stop list
            stopsList = null;
            try
            {
                var busStops = await Models.Api.Route.GetOne(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), routeId);
                if(busStops.Count == 0)
                {
                    GeneralLog.Text = "There are not stops for this route";
                    return;
                }
                stopsList = busStops;
            }
            catch (Exception ex)
            {
                //TODO Connection error
                GeneralLog.Text = $"Connection error getting stops "+ex.Message;
                return;
            }
        }
    }
}
