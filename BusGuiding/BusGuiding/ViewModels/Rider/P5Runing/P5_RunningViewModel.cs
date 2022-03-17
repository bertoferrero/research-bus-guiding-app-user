using Acr.UserDialogs;
using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver.P5Running
{
    public partial class P5_RunningViewModel : BaseViewModel
    {
        private readonly Services.IMessageService _messageService;
        protected string currentStatus = "";
        public string CurrentStatus
        {
            get
            {
                return currentStatus;
            }
            set
            {
                SetProperty(ref currentStatus, value);
            }
        }

        private string vehicleId = null;
        private int runningPhase = 0;


        public P5_RunningViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            subscribeToNotificationEvent();
            initShellNavigationEvents();
        }

        private void resetView()
        {
            vehicleId = null;
            runningPhase = 0;
            currentStatus = "";
        }

        protected async Task  dismissRequests()
        {
            //Stops
            var taskInvalidateStops = Models.Api.StopRequest.InvalidatePendentRequestsAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""));
            //Notifications
            var taskInvalidateNotifications = Models.Api.NotificationTopics.ClearNotificationTokenListAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""));

            await taskInvalidateStops;
            await taskInvalidateNotifications;
        }  

        protected async void initPhase11()
        {
            await LoadingPopupPage.ShowLoading();
            try
            {

                //Set phase
                runningPhase = 11;

                //Reset server requests
                await dismissRequests();

                //Request in_transit_to and incoming_at for line and origin stop
                List<string> topics = new List<string>();
                topics.Add($"line.{RouteSchemaId}.in_transit_to.{OriginStopSchemaId}");
                topics.Add($"line.{RouteSchemaId}.incoming_at.{OriginStopSchemaId}");
                _ = Models.Api.NotificationTopics.SubscribeNotificationTokenListAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), topics);

                //Request for the stop at the origin bus stop
                _ = Models.Api.StopRequest.RequestRouteStopAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), RouteSchemaId, OriginStopSchemaId);

                //Set status message
                CurrentStatus = "Waiting for an incoming bus";

            }
            catch (Exception ex)
            {
                //TODO mostrar error y echar atrás al cerrarse
                UserDialogs.Instance.Alert($"Sorry, a technical error happend during phase {runningPhase}");
                await Shell.Current.GoToAsync("//rider");
            }
            finally
            {
                await LoadingPopupPage.HideLoadingAsync();
            }
        }

        protected async void initPhase12(string vehicleId)
        {
            try
            {
                //Just in case
                if(runningPhase == 12)
                {
                    return;
                }

                //Set phase
                runningPhase = 12;

                //Save vehicle
                this.vehicleId = vehicleId;

                //Request in_transit_to to any stop
                List<string> topics = new List<string>();
                topics.Add($"vehicle.{vehicleId}.in_transit_to.0");
                _ = Models.Api.NotificationTopics.SubscribeNotificationTokenListAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), topics);


            }
            catch (Exception ex)
            {
                //TODO mostrar error y echar atrás al cerrarse
                UserDialogs.Instance.Alert($"Sorry, a technical error happend during phase {runningPhase}");
                await Shell.Current.GoToAsync("//rider");
            }
        }

        protected async void initPhase21()
        {
            try
            {
                //Just in case
                if (runningPhase == 21)
                {
                    return;
                }

                //Set phase
                runningPhase = 21;

                //Clear any pending request
                await dismissRequests();

                //Request for the stop at the destination bus stop
                _ = Models.Api.StopRequest.RequestVehicleStopAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), vehicleId, DestinationStopSchemaId);

                //Request notificiations
                List<string> topics = new List<string>();
                topics.Add($"vehicle.{vehicleId}.in_transit_to.0");
                topics.Add($"vehicle.{vehicleId}.incoming_at.{DestinationStopSchemaId}");
                _ = Models.Api.NotificationTopics.SubscribeNotificationTokenListAsync(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), topics);


            }
            catch (Exception ex)
            {
                //TODO mostrar error y echar atrás al cerrarse
                UserDialogs.Instance.Alert($"Sorry, a technical error happend during phase {runningPhase}");
                await Shell.Current.GoToAsync("//rider");
            }
        }

    }

}
