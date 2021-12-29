using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class NotificationLog : BasicRest
    {

        public static async Task<Dictionary<string, string>> UpdateNotificationLog(string apiToken, int logId, Dictionary<string, string> data)
        {
            return await ExecuteRequest< Dictionary<string, string>> ($"{Constants.Api.NotificationLog}/{logId}", Method.POST, apiToken, data);
        }
        public static async Task<Dictionary<string, string>> UpdateNotificationLog(string apiToken, int logId, DateTime deliveryTime)
        {
            Dictionary<string, string> data = new Dictionary<string, string>() { { "delivery_date", deliveryTime.ToString("yyyy-MM-dd HH:mm:ss")       } };
            return await ExecuteRequest<Dictionary<string, string>>($"{Constants.Api.NotificationLog}/{logId}", Method.POST, apiToken, data);
        }

    }
}
