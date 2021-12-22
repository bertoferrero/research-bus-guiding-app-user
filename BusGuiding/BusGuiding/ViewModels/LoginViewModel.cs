using BusGuiding.Models.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusGuiding.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        private string username;
        private string password;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            //IsBusy = true;
            try
            {
                await Models.Api.User.LoginAsync(username, password);
            }catch(ConnectionException ex)
            {
                //TODO Connection error
            }catch(StatusCodeException ex)
            {
                //Login error
            }
            //IsBusy = false;
        }
    }
}
