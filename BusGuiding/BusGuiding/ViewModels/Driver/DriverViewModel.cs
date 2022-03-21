using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Resources;
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
                ViewTitle = value ? DriverTexts.TitleRunningView : DriverTexts.TitleChoosingView;
            }
        }

        private string viewTitle = DriverTexts.TitleChoosingView;
        public string ViewTitle
        {
            get
            {
                return viewTitle;
            }
            set{
                SetProperty(ref viewTitle, value);
            }
        }


        public DriverViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            StartCommand = new Command(OnStartClicked);
            FinishCommand = new Command(OnFinishClicked);
            _ = initialiseAsync();
        }

        ~DriverViewModel()
        {
            try
            {
                runningFinalize();
            }
            catch(Exception ex) { }
        }

        partial void runningFinalize();


    }
}
