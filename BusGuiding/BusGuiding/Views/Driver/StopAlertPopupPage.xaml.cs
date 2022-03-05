using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusGuiding.Views.Driver
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopAlertPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public StopAlertPopupPage(string stopId)
        {
            InitializeComponent();
            StopIdLabel.Text = stopId;
        }

        public static async Task ShowStopAlertAsync(string stopId)
        {
            await CloseStopAlertAsync();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new StopAlertPopupPage(stopId));
        }

        public static async Task CloseStopAlertAsync()
        {
            try
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            _ = CloseStopAlertAsync();
        }
    }
}