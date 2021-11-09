
using BusGuiding.DependencyServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusGuiding
{
    class NotificationHandler
    {
        public static void OnNewToken(string NewToken)
        {
        }

        public static void OnNewNotification(Dictionary<string, string> data) { }

        public static async Task<string> GetTokenAsync()
        {
            INotificationService service = DependencyService.Get<INotificationService>();
            string token = await service.GetDeviceTokenAsync();
            return token;
        }
    }
}
