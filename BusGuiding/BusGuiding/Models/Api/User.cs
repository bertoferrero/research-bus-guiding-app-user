using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class User : BasicRest
    {
        public static async Task<Dictionary<string, string>> LoginAsync(string user, string password)
        {
            //https://www.luisllamas.es/consumir-un-api-rest-en-c-facilmente-con-restsharp/
            Dictionary<string, string> authData = new Dictionary<string, string> { { "username", user }, { "password", password } };
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.LoginPath, Method.POST, null, authData);
        }

        public static async Task<Dictionary<string, string>> GetUserAsync(string apiToken)
        {
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.UserPath, Method.GET, apiToken);
        }

        public static async Task<Dictionary<string, string>> UpdateNotificationTokenAsync(string apiToken, string notificationToken)
        {
            Dictionary<string, string> data = new Dictionary<string, string> { { "notification_token", notificationToken } };
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.UserPath, Method.PUT, apiToken, data);
        }

        public static async Task<Dictionary<string, string>> UpdateDriverVehicleAndRoute(string apiToken, string vehicleId, string routeId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if(vehicleId != null)
            {
                data.Add("vehicle_id", vehicleId);
            }
            if (routeId != null)
            {
                data.Add("route_id", routeId);
            }
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.UserPath, Method.PUT, apiToken, data);
        }
        public static async Task<Dictionary<string, string>> UpdateDriverLatitudeLongitude(string apiToken, double latitude, double longitude)
        {
            Dictionary<string, double> data = new Dictionary<string, double> { { "lat", latitude }, { "lon", longitude } };
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.UserPath, Method.PUT, apiToken, data);
        }
    }
}
