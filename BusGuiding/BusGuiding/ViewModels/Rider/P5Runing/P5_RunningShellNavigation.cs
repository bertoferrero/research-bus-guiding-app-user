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
        protected void initShellNavigationEvents()
        {
            Shell.Current.Navigated += Current_Navigated;
            Shell.Current.Navigating += Current_Navigating;
        }

        protected void stopShellNavigationEvents()
        {
            Shell.Current.Navigated -= Current_Navigated;
            Shell.Current.Navigating -= Current_Navigating;
        }

        private void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            if (e.Current.Location.OriginalString.EndsWith("/p5"))
            {
                //It does not work
                /*
                //Exiting from the view
                if (e.CanCancel)
                {
                    bool confirm = await UserDialogs.Instance.ConfirmAsync("Cancel current route?", null, "Yes", "No");
                    if (!confirm)
                    {
                        e.Cancel();
                        return;
                    }
                }*/
                //Disconnect event listeners
                stopShellNavigationEvents();
                unsubscribeToNotificationEvent();

                //Resetting the view and stopping all the pending requests
                _ = dismissRequests();
                resetView();
            }
        }

        private void Current_Navigated(object sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.EndsWith("/p5"))
            {
                //Entered into the running view, the magic starts
                //Call init phase 1.1
                initPhase11();
            }
        }
    }

}
