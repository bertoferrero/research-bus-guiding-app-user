using BusGuiding.ViewModels.Driver;
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
    public partial class DriverPage : ContentPage
    {
        public DriverPage()
        {
            InitializeComponent();
            this.BindingContext = new DriverViewModel();
        }

        protected override void OnDisappearing()
        {
            (this.BindingContext as DriverViewModel).OnDisappearing();
            base.OnDisappearing();
            
        }
    }
}