using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusGuiding.Models.Api
{
    public class Stop : BasicRest
    {       
        public static async Task<List<Dictionary<string, string>>> GetAll(string apiToken)
        {
            return await ExecuteRequest<List<Dictionary<string, string>>>(Constants.Api.StopPath, Method.GET, apiToken);
        }
        public static async Task<Dictionary<string, string>> GetOne(string apiToken, string schemaId)
        {
           return await ExecuteRequest<Dictionary<string, string>>($"{Constants.Api.StopPath}/stopcode/{schemaId}", Method.GET, apiToken);
        }
        public static async Task<Dictionary<string, string>> GetOneByStopCode(string apiToken, string stopId)
        {
            return await ExecuteRequest<Dictionary<string, string>>($"{Constants.Api.StopPath}/{stopId}", Method.GET, apiToken);
        }
        public static async Task<Dictionary<string, string>> GetNearest(string apiToken, double latitude, double longitude)
        {
            string latitudeStr = latitude.ToString().Replace(',', '.');
            string longitudeStr = longitude.ToString().Replace(',', '.');
            return await ExecuteRequest<Dictionary<string, string>>($"{Constants.Api.StopPath}/nearest/{latitudeStr}/{longitudeStr}", Method.GET, apiToken);
        }
        public static async Task<List<Dictionary<string, string>>> GetRoutes(string apiToken, string stopId)
        {
            return await ExecuteRequest<List<Dictionary<string, string>>>($"{Constants.Api.StopPath}/{stopId}/routes", Method.GET, apiToken);
        }
    }
}
