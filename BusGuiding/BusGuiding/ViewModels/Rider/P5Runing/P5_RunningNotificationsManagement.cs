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

            }
        }

        private void Phase11NotificationHandler(IDictionary<string, string> e)
        {
            if (e["status"].ToUpper().Equals("IN_TRANSIT_TO"))
            {
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.in_transit_to.{OriginStopCode}");
                sendPhase1InTransitToMessages();
                initPhase12(e["vehicle_id"]);
            }
            else if (e["status"].ToUpper().Equals("INCOMING_AT"))
            {
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.incoming_at.{OriginStopCode}");
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.in_transit_to.{OriginStopCode}");
                sendPhase1IncomingAtMessages();
                initPhase12(e["vehicle_id"]);
            }
        }

        private void Phase12NotificationHandler(IDictionary<string, string> e)
        {
            if (e["status"].ToUpper().Equals("INCOMING_AT"))
            {
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.incoming_at.{OriginStopCode}");
                easyUnsuscribeNotificationToken($"line.{RouteSchemaId}.in_transit_to.{OriginStopCode}");
                sendPhase1IncomingAtMessages();           
            }
            else if (e["status"].ToUpper().Equals("IN_TRANSIT_TO"))
            {
                if (!e["stop_id"].Equals(OriginStopSchemaId))
                {
                    easyUnsuscribeNotificationToken($"vehicle.{vehicleId}.in_transit_to.0");
                }
                /*sendPhase1IncomingAtMessages();
                initPhase12(e["vehicle_id"]);*/
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
            CurrentStatus = "Bus is incoming at the origin stop.";
            NotificationHandler.Instance.ShowNotification("Route update", "Your bus is incoming at the origin stop, be prepared.");
        }

        //Helpers
        private void easyUnsuscribeNotificationToken(string token)
        {
            _ = Models.Api.NotificationTopics.UnsubscribeNotificationTokenListAsync(
                    Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""),
                    new List<string>() { token }
                    );
        }
    }

}
