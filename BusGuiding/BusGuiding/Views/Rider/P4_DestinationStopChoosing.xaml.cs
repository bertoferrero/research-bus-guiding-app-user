using BusGuiding.ViewModels.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusGuiding.Views.Rider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class P4_DestinationStopChoosing : ContentPage
    {
        public P4_DestinationStopChoosing()
        {
            InitializeComponent();
            BindingContext = new P4_DestinationStopChoosingViewModel();
        }
    }
}