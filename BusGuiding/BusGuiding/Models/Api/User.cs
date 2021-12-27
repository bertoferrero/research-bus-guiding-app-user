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
    }
}
