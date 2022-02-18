using System;
using System.Collections.Generic;
using System.Text;

namespace BusGuiding.Constants
{
    public static class Api
    {
        public static string ApiEndpoint => "http://9d69-185-196-57-199.ngrok.io/api";// "http://research.busguiding.bertoferrero.com/api";
        public static string LoginPath => "/login";
        public static string LogoutPath => "/logout";
        public static string UserPath => "/user";
        public static string NotificationTopicsPath => "/user/notificationtopics";
        public static string SampleLog => "/samplelog";
        public static string NotificationLog => "/notificationlog";
        public static string RoutePath => "/route";
        public static string StopRequestPath => "/stoprequest";
    }
}
