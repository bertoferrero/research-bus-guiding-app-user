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
    public partial class GTFSTestPage : ContentPage
    {

        public GTFSTestPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void StoppedButton_Clicked(object sender, EventArgs e)
        {

        }

        private void NextStopButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
