using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class Route : BasicRest
    {       
        public static async Task<List<Dictionary<string, string>>> GetAll(string apiToken)
        {
            return await ExecuteRequest<List<Dictionary<string, string>>>(Constants.Api.RoutePath, Method.GET, apiToken);
        }
        public static async Task<List<Dictionary<string, string>>> GetOne(string apiToken, string routeId)
        {
            return await ExecuteRequest<List<Dictionary<string, string>>>($"{Constants.Api.RoutePath}/{routeId}/stops", Method.GET, apiToken);
        }
    }
}
