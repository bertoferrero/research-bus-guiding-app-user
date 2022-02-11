using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly Services.IMessageService _messageService;

        public Command LoginCommand { get; }
        private bool showForm = false;
        private string username = "";
        private string password = "";

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

        public bool ShowForm
        {
            get => showForm;
            set => SetProperty(ref showForm, value);
        }

        public LoginViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            LoginCommand = new Command(OnLoginClicked);

        }

        public async void CheckLoggedUser()
        {
            string userApiToken = Preferences.Get(Constants.PreferenceKeys.UserApiToken, "");
            if (!string.IsNullOrEmpty(userApiToken))
            {
                await LoadingPopupPage.ShowLoading();
                //Si hay token, comprobamos si es valido, de ser asi enviamos a la pagina principal. Si no, lo borramos
                try
                {
                    //Try to update the device notification token on the same request
                    string currentNotificationToken = await NotificationHandler.Instance.GetTokenAsync();
                    //await Models.Api.User.GetUserAsync(userApiToken);
                    await Models.Api.User.UpdateNotificationTokenAsync(userApiToken, currentNotificationToken);
                    //Cargamos la pagina principal
                    await LoadingPopupPage.HideLoadingAsync();
                    (App.Current.MainPage as AppShell).SetLoggedUserContextAsync();
                    return;
                }
                catch (Exception ex)
                {
                    await LoadingPopupPage.HideLoadingAsync();
                    Preferences.Remove(Constants.PreferenceKeys.UserApiToken);
                }
            }
            ShowForm = true;
        }

        private async void OnLoginClicked(object obj)
        {

            string cleanUsername = username.Trim();
            string cleanPassword = password.Trim();
            if (string.IsNullOrEmpty(cleanUsername) || string.IsNullOrEmpty(cleanPassword))
            {
                return;
            }
            try
            {
                await LoadingPopupPage.ShowLoading();
                var loginResponse = await Models.Api.User.LoginAsync(cleanUsername, cleanPassword);
                //Store the token
                Preferences.Set(Constants.PreferenceKeys.UserApiToken, loginResponse["token"]);
                //Store the role
                Preferences.Set(Constants.PreferenceKeys.UserRole, loginResponse["role"]);
                //Send current notification token
                string currentNotificationToken = await NotificationHandler.Instance.GetTokenAsync();
                await Models.Api.User.UpdateNotificationTokenAsync(loginResponse["token"], currentNotificationToken);
                //Load the page
                await LoadingPopupPage.HideLoadingAsync();
                (App.Current.MainPage as AppShell).SetLoggedUserContextAsync();
            }
            catch(ConnectionException ex)
            {
                //TODO Connection error
                await LoadingPopupPage.HideLoadingAsync();
                await this._messageService.DisplayAlert("Error", "Connexion error.", "Close");
            }
            catch(StatusCodeException ex)
            {
                //Login error
                await LoadingPopupPage.HideLoadingAsync();
                await this._messageService.DisplayAlert("Login error", "Something is wrong in your data.", "Close");
            }
            //IsBusy = false;
        }
    }
}
