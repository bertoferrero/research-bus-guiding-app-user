using Acr.UserDialogs;
using BusGuiding.Models.Api.Exceptions;
using BusGuiding.Views.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BusGuiding.ViewModels.Driver.P5Running
{
    public partial class P5_RunningViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            //asignar todas las propiedades
            //origin_stop_code={stopCode}&origin_stop_name={stopName}&origin_stop_schema_id={stopSchemaId}&route_schema_id={RouteSchemaId}&route_name={RouteName}&route_color={RouteColor}&destination_stop_code={SelectedStop.Code}&destination_stop_name={SelectedStop.Name}&destination_stop_schema_id={SelectedStop.SchemaId}&subroute={JsonSerializer.Serialize(subRoute)}"
            OriginStopName = HttpUtility.UrlDecode(query["origin_stop_name"]);
            OriginStopCode = HttpUtility.UrlDecode(query["origin_stop_code"]);
            OriginStopSchemaId = HttpUtility.UrlDecode(query["origin_stop_schema_id"]);
            RouteSchemaId = HttpUtility.UrlDecode(query["route_schema_id"]);
            RouteName = HttpUtility.UrlDecode(query["route_name"]);
            RouteColor = HttpUtility.UrlDecode(query["route_color"]);
            DestinationStopCode = HttpUtility.UrlDecode(query["destination_stop_code"]);
            DestinationStopName = HttpUtility.UrlDecode(query["destination_stop_name"]);
            DestinationStopSchemaId = HttpUtility.UrlDecode(query["destination_stop_schema_id"]);
            Subroute = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(HttpUtility.UrlDecode(query["subroute"]));

        }

        public string OriginStopCode { get; private set; }

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
        public string OriginStopSchemaId { get; private set; }

        public string RouteSchemaId { get; private set; }

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

        public string DestinationStopCode { get; private set; }

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
        protected string DestinationStopSchemaId { get; private set; }

        public List<Dictionary<string, string>> Subroute { get; private set; }




    }

}
