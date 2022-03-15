using BusGuiding.ViewModels.Driver.P5Running;
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
    public partial class P5_running : ContentPage
    {
        public P5_running()
        {
            InitializeComponent();
            this.BindingContext = new P5_RunningViewModel();
        }

        //TODO check when back ispressed, even physical or not, confirm and send the dismiss signal
    }
}