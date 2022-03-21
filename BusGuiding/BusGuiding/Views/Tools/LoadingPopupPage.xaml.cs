using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusGuiding.Views.Tools
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public LoadingPopupPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return true;
        }

        public static async Task ShowLoading()
        {
            //TODO this is a patch, this line must to be moved out from this class
            UserDialogs.Instance.ShowLoading(BusGuiding.Resources.GeneralTexts.Loading);
            //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new LoadingPopupPage());
        }

        public static async Task HideLoadingAsync()
        {
            try
            {
                //TODO his is a patch
                UserDialogs.Instance.HideLoading();
                //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            catch(Exception ex)
            {

            }
        }
    }
}