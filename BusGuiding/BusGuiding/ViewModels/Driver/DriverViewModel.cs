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
            }
        }


        public DriverViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            StartCommand = new Command(OnStartClicked);
            _ = initialiseAsync();
        }

        partial void switchToWorkingScreen(); //Called when its time to show the working screen


    }
}
