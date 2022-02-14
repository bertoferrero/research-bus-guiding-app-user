using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class StopRequest : BasicRest
    {

        public static async Task<Dictionary<string, string>> PendentStopRequestedListAsync(string apiToken)
        {
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.StopRequestPath, Method.GET, apiToken);
        }
        public static async Task<Dictionary<string, string>> RequestVehicleStopAsync(string apiToken, string vehicleId, string stopId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>() {
                { "stop_id", stopId },
                { "vehicle_id", vehicleId }
            };
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.StopRequestPath, Method.POST, apiToken, data);
        }
        public static async Task<Dictionary<string, string>> RequestRouteStopAsync(string apiToken, string routeId, string stopId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>() {
                { "stop_id", stopId },
                { "route_id", routeId }
            };
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.StopRequestPath, Method.POST, apiToken, data);
        }
        public static async Task<Dictionary<string, string>> InvalidatePendentRequestsAsync(string apiToken)
        {
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.StopRequestPath, Method.DELETE, apiToken);
        }
        

    }
}
