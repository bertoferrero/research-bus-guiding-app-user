using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusGuiding.Views.Dev
{
    public partial class DelayTestsPage : ContentPage
    {
        public DelayTestsPage()
        {
            InitializeComponent();
        }

        private async void FCM_Button_ClickedAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("fcm");
        }

        private void GTFS_Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
