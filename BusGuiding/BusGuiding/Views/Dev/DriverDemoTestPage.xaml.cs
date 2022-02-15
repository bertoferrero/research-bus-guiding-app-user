using BusGuiding.Models.Api.Exceptions;
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

        private void StartButton_ClickedAsync(object sender, EventArgs e)
        {
            if (testRunning)
            {
                stopTest();
            }
            else
            {
                startTest();
            }
        }

        private async void startTest()
        {
            string routeId = RouteIdEntry.Text.Trim();
            string vehicleId = VehicleIdEntry.Text.Trim();
            string initialStop = InitialStop.Text.Trim();
            string finalStop = FinalStop.Text.Trim();
            if (String.IsNullOrEmpty(routeId) || String.IsNullOrEmpty(vehicleId) || String.IsNullOrEmpty(initialStop) || String.IsNullOrEmpty(finalStop))
            {
                GeneralLog.Text = "Route, Stops and Vehicle fields must be filled";
                return;
            }
            GeneralLog.Text = "";
            List<Dictionary<string,string>> busStops;
            try
            {
                busStops = await Models.Api.Route.GetOne(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), routeId);
                if(busStops.Count == 0)
                {
                    GeneralLog.Text = "There are not stops for this route";
                    return;
                }
                //Search lastStopIdIndex
                bool initialStopFound = false;
                bool finalStopFound = false;
                foreach(Dictionary<string, string> stop in busStops) { 
                    if(stop["code"] == initialStop)
                    {
                        initialStopFound = true;
                    }
                    if (stop["code"] == finalStop)
                    {
                        finalStopFound = true;
                    }
                    if(initialStopFound && finalStopFound)
                    {
                        break;
                    }
                }
                if(!initialStopFound || !finalStopFound)
                {
                    GeneralLog.Text = "The selected stops are not in the route's list";
                    return;
                }
            }
            catch (Exception ex)
            {
                //TODO Connection error
                GeneralLog.Text = $"Connection error getting stops "+ex.Message;
                return;
            }

            //Everything is ok, send link between user and vehicle
            if(!await SendVehicleAndRouteAsync(vehicleId, routeId))
            {
                return;
            }

            //register events of NotificanHandler
            NotificationHandler.Instance.NewNotification += NotificationHandler_NewNotification;

            //Request the stop for each stop between the initial and final ones
            bool sendRequest = false;
            try
            {
                do
                {
                    foreach (Dictionary<string, string> stop in busStops)
                    {
                        if (!sendRequest)
                        {
                            if(stop["code"] == initialStop)
                            {
                                sendRequest = true;
                            }
                        }


                        if (sendRequest)
                        {
                            _ = Models.Api.StopRequest.RequestVehicleStopAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), vehicleId, stop["code"]);

                            if (stop["code"] == finalStop)
                            {
                                sendRequest = false;
                                break;
                            }
                        }
                    }
                } while (sendRequest);
            }
            catch (Exception ex)
            {
                //TODO Connection error
                GeneralLog.Text = $"Connection error getting stops " + ex.Message;
                return;
            }

            //Display change
            RouteIdEntry.IsEnabled = VehicleIdEntry.IsEnabled = InitialStop.IsEnabled = FinalStop.IsEnabled = false;
            StoppedButton.IsVisible = true;
            StartButton.Text = "Stop";
            testRunning = true;
        }

        private void stopTest()
        {

            //unregister events from NotificationHandler
            NotificationHandler.Instance.NewNotification -= NotificationHandler_NewNotification;
            //invalidate pendent requests
            _ = Models.Api.StopRequest.InvalidatePendentRequestsAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""));
            //Change screen
            RouteIdEntry.IsEnabled = VehicleIdEntry.IsEnabled = InitialStop.IsEnabled = FinalStop.IsEnabled = true;
            StoppedButton.IsVisible = false;
            StartButton.Text = "Start";
            testRunning = false;
        }

        private void NotificationHandler_NewNotification(object sender, IDictionary<string, string> e)
        {
            if(e["notification_type"] != "StopRequest")
            {
                return;
            }
            var extraData = $"vehicle_id: {e["vehicle_id"]}, line_id: {e["line_id"]}, status: {e["status"]}, stop_id:{e["stop_id"]}";
            GeneralLog.Text = "Notificacion de parada recibida";
            _ = SendSampleAsync("t2.1_notification_received", extraData);
        }

        private void StoppedButton_Clicked(object sender, EventArgs e)
        {
            _ = SendSampleAsync("t2.1_vehicle_stop_signal");
            GeneralLog.Text = "Stop signal sent";
        }


        private async Task<bool> SendVehicleAndRouteAsync(string vehicleId, string routeId)
        {
            try
            {
                GeneralLog.Text = $"Sending vehicle and route";
                Dictionary<string, string> result = await Models.Api.User.UpdateDriverVehicleAndRoute(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), vehicleId, routeId);
                GeneralLog.Text = $"Sent vehicle and route";
                return true;
            }
            catch (ConnectionException ex)
            {
                //TODO Connection error
                GeneralLog.Text = $"Connection error " + ex.Message;
                return false;
            }
            catch (StatusCodeException ex)
            {
                //Login error
                GeneralLog.Text = $"Status code error " + ex.Message;
                return false;
            }
        }

        private async Task SendSampleAsync(string sampleType, string extraData = "")
        {
            try
            {
                GeneralLog.Text = $"Sending {sampleType}";
                await Models.Api.SampleLog.AddSampleLog(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), sampleType, DateTime.UtcNow, extraData);
                GeneralLog.Text = $"Sent {sampleType}";
            }
            catch (ConnectionException ex)
            {
                //TODO Connection error
                GeneralLog.Text = $"Connection error {sampleType}";
            }
            catch (StatusCodeException ex)
            {
                //Login error
                GeneralLog.Text = $"Status code error {sampleType}";
            }
        }


    }
}
