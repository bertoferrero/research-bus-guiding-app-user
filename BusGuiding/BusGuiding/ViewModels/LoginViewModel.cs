using BusGuiding.Models.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
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
            string userApiToken = Preferences.Get(Constants.PreferenceKeys.UserApiToken, "");
            if (!string.IsNullOrEmpty(userApiToken))
            {
                //TODO Si hay token, comprobamos si es valido, de ser asi enviamos a la pagina principal. Si no, lo borramos
            }
            LoginCommand = new Command(OnLoginClicked);


            //DEBUG
            Preferences.Set(Constants.PreferenceKeys.UserRole, "dev");
            (App.Current.MainPage.BindingContext as ShellViewModel).SetLoggedUserContextAsync();
        }

        private async void OnLoginClicked(object obj)
        {
            //IsBusy = true;
            try
            {
                var loginResponse = await Models.Api.User.LoginAsync(username, password);
                //Store the token
                Preferences.Set(Constants.PreferenceKeys.UserApiToken, loginResponse["token"]);
                //Store the role
                Preferences.Set(Constants.PreferenceKeys.UserRole, loginResponse["role"]);
                //TODO Load the page
                //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            catch(ConnectionException ex)
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
