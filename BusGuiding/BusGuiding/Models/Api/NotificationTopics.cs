using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class NotificationTopics : BasicRest
    {

        public static async Task<List<string>> SubscribedNotificationTokenListAsync(string apiToken)
        {
            return await ExecuteRequest<List<string>>(Constants.Api.NotificationTopicsPath, Method.GET, apiToken);
        }
        public static async Task<List<string>> SubscribeNotificationTokenListAsync(string apiToken, List<string> topics)
        {
            return await ExecuteRequest<List<string>>(Constants.Api.NotificationTopicsPath, Method.POST, apiToken, topics);
        }
        public static async Task<List<string>> UnsubscribeNotificationTokenListAsync(string apiToken, List<string> topics)
        {
            return await ExecuteRequest<List<string>>(Constants.Api.NotificationTopicsPath, Method.DELETE, apiToken, topics);
        }
        public static async Task<List<string>> ClearNotificationTokenListAsync(string apiToken)
        {
            List<string> topics = new List<string>() { "*" };
            return await ExecuteRequest<List<string>>(Constants.Api.NotificationTopicsPath, Method.DELETE, apiToken, topics);
        }

    }
}
