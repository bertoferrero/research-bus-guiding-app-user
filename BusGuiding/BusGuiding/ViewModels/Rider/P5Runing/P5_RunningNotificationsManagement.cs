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
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.incoming_at.{OriginStopSchemaId}");
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.in_transit_to.{OriginStopSchemaId}");
                sendPhase1IncomingAtMessages();
                initPhase12(e["vehicle_id"]);
            }
        }

        private void Phase12NotificationHandler(IDictionary<string, string> e)
        {
            if (e["status"].ToUpper().Equals("INCOMING_AT"))
            {
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.incoming_at.{OriginStopSchemaId}");
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.in_transit_to.{OriginStopSchemaId}");
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
                    easyUnsuscribeNotificationToken($"vehicle.{vehicleId}.in_transit_to.0");
                    //TODO init phase22
                }
            }
            else if (e["status"].ToUpper().Equals("INCOMING_AT"))
            {
                easyUnsuscribeNotificationToken($"vehicle.{vehicleId}.incoming_at.{DestinationStopSchemaId}");
                sendPhase2Incoming();
            }
        }

        //Messages To User
        private void sendPhase1InTransitToMessages()
        {
            CurrentStatus = "Bus in transit to the origin stop.";
            NotificationHandler.Instance.ShowNotification("Route update", "A bus for you has been located, it is now in transit to the origin stop.");
        }
        private void sendPhase1IncomingAtMessages()
        {
            CurrentStatus = "Bus is about to arrive at the origin stop.";
            NotificationHandler.Instance.ShowNotification("Route update", "Your bus is incoming at the origin stop, be prepared.");
        }

        private void sendPhase2RemainingStopsMessages(string nextStopName, int remainingStopsAmount)
        {
            if(remainingStopsAmount > 0)
            {
                //Middle stop
                var stopsPlural = (remainingStopsAmount > 1 ? "stops" : "stop");
                CurrentStatus = $"In route. {remainingStopsAmount} {stopsPlural} before until destination. Next stop is {nextStopName}";
            }
            else
            {
                //Final stop
                CurrentStatus = $"In transit to destination stop.";
                NotificationHandler.Instance.ShowNotification("Route update", "Your bus is now in transit to destination, be prepared to get off.");
            }
        }

        private void sendPhase2Incoming()
        {
            CurrentStatus = $"Arriving at destination stop.";
            NotificationHandler.Instance.ShowNotification("Route update", "Your bus is about to arrive at destination, be prepared to get off.");

        }

        //Helpers
        private void easyUnsuscribeNotificationToken(string token)
        {
            _ = Models.Api.NotificationTopics.UnsubscribeNotificationTokenListAsync(
                    Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""),
                    new List<string>() { token }
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
