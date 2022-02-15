using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusGuiding.Views.Dev
{
    public partial class FunctionalDemoTestsPage : ContentPage
    {
        public FunctionalDemoTestsPage()
        {
            InitializeComponent();
        }

        private async void Driver_Button_ClickedAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("driver");
        }

        private async void Rider_Button_ClickedAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("rider");
        }
    }
}
