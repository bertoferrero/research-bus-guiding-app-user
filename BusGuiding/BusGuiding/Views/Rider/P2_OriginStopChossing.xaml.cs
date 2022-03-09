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
    public partial class P2_OriginStopChossing : ContentPage
    {
        public P2_OriginStopChossing()
        {
            InitializeComponent();
            this.BindingContext = new P2_originStopChossingViewModel();
        }
    }
}