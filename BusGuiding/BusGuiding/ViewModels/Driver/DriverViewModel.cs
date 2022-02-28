using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver
{
    public partial class DriverViewModel : BaseViewModel
    {
        private readonly Services.IMessageService _messageService;

        private bool isRunning = false;

        public bool IsRunning
        {
            get => isRunning;
            set {
                SetProperty(ref showChoosingRouteForm, value);
                ShowChoosingRouteForm = !value;
                ShowRunningForm = value;
            }
        }


        public DriverViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            StartCommand = new Command(OnStartClicked);
            FinishCommand = new Command(OnFinishClicked);
            _ = initialiseAsync();
        }

        public void OnDisappearing()
        {
            IsRunning = false;
        }


    }
}
