using Acr.UserDialogs;
using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver.P5Running
{
    /*
     Phases: Phase 1 -> waiting for the bus, Phase 2 -> in route
     
      Phase 1.1: System is waiting for the notification of in_transint_to o incoming_to from any bus the de origin stop. When it happens, the vehicle id is caught and starts phase 1.2
      Phase 1.2: System waits for the notification of in_transit_to to a new stop. This means the user is inside and the bus in transit to his destionation, which is the phase 2
      Phase 2.1 System is waiting to in_transit_to to any stop. This is used for notifying user about the remaining amount of stop until his destionation. 
                When is received in_transit_to destination, starts phase 2.2
                Systems also waits for incoming_at signal to destination stop. Once received it, the system properly notifies the user to be ready to soon get off the bus.
      Phase 2.2: Like Phase 1.2, system waits for the notification of in_transit_to to a new stop. This will mean the user got off the bus so, the guiding can conclude.
     */

    public partial class P5_RunningViewModel
    {
        
        private void subscribeToNotificationEvent()
        {
            NotificationHandler.Instance.NewNotification += NewNotification;
        }
        private void unsubscribeToNotificationEvent()
        {
            NotificationHandler.Instance.NewNotification -= NewNotification;
        }

        private void NewNotification(object sender, IDictionary<string, string> e)
        {
            if (e["notification_type"] != "VehiclePosition")
            {
                return;
            }
            switch (runningPhase)
            {
                case 11:
                    Phase11NotificationHandler(e);
                    break;
                case 12:
                    Phase12NotificationHandler(e);
                    break;
                case 21:
                    Phase21NotificationHandler(e);
                    break;
                case 22:
                    Phase22NotificationHandler(e);
                    break;

            }
        }

        private void Phase11NotificationHandler(IDictionary<string, string> e)
        {
            if (e["status"].ToUpper().Equals("IN_TRANSIT_TO"))
            {
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.in_transit_to.{OriginStopSchemaId}");
                sendPhase1InTransitToMessages();
                initPhase12(e["vehicle_id"]);
            }
            else if (e["status"].ToUpper().Equals("INCOMING_AT"))
            {
                easyUnsuscribeNotificationTokenList(new List<string>() {
                    $"line.{RouteSchemaId}.incoming_at.{OriginStopSchemaId}",
                    $"line.{RouteSchemaId}.in_transit_to.{OriginStopSchemaId}" }
                );
                sendPhase1IncomingAtMessages();
                initPhase12(e["vehicle_id"]);
            }
        }

        private void Phase12NotificationHandler(IDictionary<string, string> e)
        {
            if (e["status"].ToUpper().Equals("INCOMING_AT"))
            {
                easyUnsuscribeNotificationTokenList(new List<string>() {
                    $"line.{RouteSchemaId}.incoming_at.{OriginStopSchemaId}",
                    $"line.{RouteSchemaId}.in_transit_to.{OriginStopSchemaId}" }
                );
                sendPhase1IncomingAtMessages();           
            }
            else if (e["status"].ToUpper().Equals("IN_TRANSIT_TO"))
            {
                if (!e["stop_id"].Equals(OriginStopSchemaId))
                {
                    easyUnsuscribeNotificationToken($"vehicle.{vehicleId}.in_transit_to.0");
                    initPhase21();
                    //Get next stop name and remaining stops amount
                    string stopName = "";
                    int remainingStops = getRemainingStopsSummary(e["stop_id"], out stopName);
                    //Send messages
                    sendPhase2RemainingStopsMessages(stopName, remainingStops);
                }
                /*sendPhase1IncomingAtMessages();
                initPhase12(e["vehicle_id"]);*/
            }
        }

        private void Phase21NotificationHandler(IDictionary<string, string> e)
        {
            if (e["status"].ToUpper().Equals("IN_TRANSIT_TO"))
            {
                //Get next stop name and remaining stops amount
                string stopName = "";
                int remainingStops = getRemainingStopsSummary(e["stop_id"], out stopName);
                //Send messages
                sendPhase2RemainingStopsMessages(stopName, remainingStops);
                if(remainingStops == 0)
                {
                    //easyUnsuscribeNotificationToken($"vehicle.{vehicleId}.in_transit_to.0");
                    //init phase22
                    initPhase22();
                }
            }
            else if (e["status"].ToUpper().Equals("INCOMING_AT") || e["status"].ToUpper().Equals("STOPPED_AT"))
            {
                easyUnsuscribeNotificationTokenList(new List<string>() {
                    $"vehicle.{vehicleId}.incoming_at.{DestinationStopSchemaId}",
                    $"vehicle.{vehicleId}.stopped_at.{DestinationStopSchemaId}" }
                );
                sendPhase2Incoming();
                //init phase22
                initPhase22();
            }
        }
        private void Phase22NotificationHandler(IDictionary<string, string> e)
        {
            if (e["status"].ToUpper().Equals("IN_TRANSIT_TO")) { 

                easyUnsuscribeNotificationToken("*");
                //show finish popup
                UserDialogs.Instance.Alert(Resources.RiderTexts.ArrivedAtDestinationAlert);
                //Move to root p2 page
                Shell.Current.GoToAsync("//rider");
            }
            else if (e["status"].ToUpper().Equals("INCOMING_AT") || e["status"].ToUpper().Equals("STOPPED_AT"))
            {
                easyUnsuscribeNotificationTokenList(new List<string>() { 
                    $"vehicle.{vehicleId}.incoming_at.{DestinationStopSchemaId}",
                    $"vehicle.{vehicleId}.stopped_at.{DestinationStopSchemaId}" }
                );
                sendPhase2Incoming();
            }
        }

        //Messages To User
        private void sendPhase1InTransitToMessages()
        {
            CurrentStatus = Resources.RiderTexts.StatusPh1InTransit;
            NotificationHandler.Instance.ShowNotificationRider(Resources.RiderTexts.NotificationTitleRouteUpdate, Resources.RiderTexts.NotificationPh1InTransit);
        }
        private void sendPhase1IncomingAtMessages()
        {
            CurrentStatus = Resources.RiderTexts.StatusPh1Incoming;
            NotificationHandler.Instance.ShowNotificationRider(Resources.RiderTexts.NotificationTitleRouteUpdate, Resources.RiderTexts.NotificationPh1Incoming);
        }

        private void sendPhase2RemainingStopsMessages(string nextStopName, int remainingStopsAmount)
        {
            if(remainingStopsAmount > 0)
            {
                //Middle stop
                CurrentStatus = string.Format(Resources.RiderTexts.StatusPh2RemainingStops, remainingStopsAmount + 1, nextStopName);
            }
            else
            {
                //Final stop
                CurrentStatus = Resources.RiderTexts.StatusPh2IntransitDestination;
                NotificationHandler.Instance.ShowNotificationRider(Resources.RiderTexts.NotificationTitleRouteUpdate, Resources.RiderTexts.NotificationPh2IntransitDestination);
            }
        }

        private void sendPhase2Incoming()
        {
            CurrentStatus = Resources.RiderTexts.StatusPh2IncomingDestination;
            NotificationHandler.Instance.ShowNotificationRider(Resources.RiderTexts.NotificationTitleRouteUpdate, Resources.RiderTexts.NotificationPh2IncomingDestination);

        }

        //Helpers
        private async void easyUnsuscribeNotificationToken(string token)
        {
            await Models.Api.NotificationTopics.UnsubscribeNotificationTokenListAsync(
                    Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""),
                    new List<string>() { token }
                    );
        }


        private async void easyUnsuscribeNotificationTokenList(List<string> tokens)
        {
            await Models.Api.NotificationTopics.UnsubscribeNotificationTokenListAsync(
                    Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""),
                    tokens
                    );
        }

        /// <summary>
        /// Returns the amount of remaining stops until destination
        /// </summary>
        /// <param name="stopSchemaId"></param>
        /// <param name="stopName"></param>
        /// <returns></returns>
        private int getRemainingStopsSummary(string stopSchemaId, out string stopName)
        {
            var subStop = Subroute.Select((element, index) => (element, index)).First(row => row.element["schema_id"] == stopSchemaId);
            stopName = subStop.element["stop_name"];

            var index = subStop.index + 1;
            return Subroute.Count() - index;
        }
    }

}
