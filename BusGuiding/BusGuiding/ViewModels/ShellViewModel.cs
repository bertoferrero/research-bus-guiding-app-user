using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {

        protected ObservableCollection<ShellSection> flyoutItems;
        public ObservableCollection<ShellSection> FlyoutItems
        {
            get { 
                return flyoutItems; 
            }
            set {
                SetProperty(ref flyoutItems, value); 
            }
        }

        protected Collection<ShellContent> DevFlyoutItems
        {
            get
            {
                return new Collection<ShellContent>()
            
                {
                            new ShellContent(){ Title="item 1"} 
                }
                
            ;
            }
        }

        public ShellViewModel()
        {
            FlyoutItems = new ObservableCollection<ShellSection>()
            {
                 new ShellSection() { Title = "seccion 1", Items = {
                     new ShellContent() { Content = new MainPage() }
                     } }
            };
        }

        public async void SetLoggedUserContextAsync()
        {
            var role = Preferences.Get(Constants.PreferenceKeys.UserRole, "");
            if (role.Equals(Constants.UserRoles.Dev))
            {
                await SetDevContextAsync();
            }
            else if (role.Equals(Constants.UserRoles.Rider))
            {

            }else if (role.Equals(Constants.UserRoles.Driver))
            {

            }
            else
            {
                Preferences.Clear();
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }

        public async Task SetDevContextAsync()
        {
            //FlyoutItems = DevFlyoutItems;
            //browse to dev main page
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }

    }
}
