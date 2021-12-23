
using BusGuiding.Views;
using Xamarin.Forms;

namespace BusGuiding
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }

        //Seguir las ideas de https://stackoverflow.com/questions/59569567/xamarin-forms-dynamically-add-shell-items y https://stackoverflow.com/questions/65911023/dynamically-create-list-of-flyoutitem-in-shell/66036972#66036972
    }
}