using Acr.UserDialogs;
using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver
{

    [QueryProperty(nameof(OriginStopCode), "origin_stop_code")]
    [QueryProperty(nameof(OriginStopName), "origin_stop_name")]
    [QueryProperty(nameof(OriginStopSchemaId), "origin_stop_schema_id")]
    [QueryProperty(nameof(RouteSchemaId), "route_schema_id")]
    [QueryProperty(nameof(RouteName), "route_name")]
    [QueryProperty(nameof(RouteColor), "route_color")]
    [QueryProperty(nameof(DestinationStopCode), "destination_stop_code")]
    [QueryProperty(nameof(DestinationStopName), "destination_stop_name")]
    [QueryProperty(nameof(DestinationStopSchemaId), "destination_stop_schema_id")]
    public class P5_RunningViewModel : BaseViewModel
    {
        private readonly Services.IMessageService _messageService;
        protected string currentStatus = "";
        public string CurrentStatus
        {
            get
            {
                return currentStatus;
            }
            set
            {
                SetProperty(ref currentStatus, value);
            }
        }
        protected string originStopCode = "";
        public string OriginStopCode
        {
            get
            {
                return originStopCode;
            }
            set
            {
                SetProperty(ref originStopCode, value);
            }
        }

        protected string originStopName = "";
        public string OriginStopName
        {
            get
            {
                return originStopName;
            }
            set
            {
                SetProperty(ref originStopName, value);
            }
        }
        protected string originStopSchemaId = "";
        public string OriginStopSchemaId
        {
            get
            {
                return originStopSchemaId;
            }
            set
            {
                SetProperty(ref originStopSchemaId, value);
            }
        }

        protected string routeSchemaId = "";
        public string RouteSchemaId
        {
            get
            {
                return routeSchemaId;
            }
            set
            {
                SetProperty(ref routeSchemaId, value);
            }
        }

        protected string routeName = "";
        public string RouteName
        {
            get
            {
                return routeName;
            }
            set
            {
                SetProperty(ref routeName, value);
            }
        }

        protected string routeColor = "";
        public string RouteColor
        {
            get
            {
                return routeColor;
            }
            set
            {
                SetProperty(ref routeColor, value);
            }
        }

        protected string destinationStopCode = "";
        public string DestinationStopCode
        {
            get
            {
                return destinationStopCode;
            }
            set
            {
                SetProperty(ref destinationStopCode, value);
            }
        }

        protected string destinationStopName = "";
        public string DestinationStopName
        {
            get
            {
                return destinationStopName;
            }
            set
            {
                SetProperty(ref destinationStopName, value);
            }
        }
        protected string destinationStopSchemaId = "";
        public string DestinationStopSchemaId
        {
            get
            {
                return destinationStopSchemaId;
            }
            set
            {
                SetProperty(ref destinationStopSchemaId, value);
            }
        }



        public P5_RunningViewModel()
        {
            this._messageService = DependencyService.Get<Services.IMessageService>();
            resetView();
        }

        public void resetView()
        {
        }        

    }

}
