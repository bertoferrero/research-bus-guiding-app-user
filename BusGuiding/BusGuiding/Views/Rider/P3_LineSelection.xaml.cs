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
    public partial class P3_LineSelection : ContentPage
    {
        public P3_LineSelection()
        {
            InitializeComponent();
            BindingContext = new P3_LineSelectionViewModel();
        }
    }
}