
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
        private static INotificationService currentService = null;
        public static void initEvents()
        {
            if (currentService == null)
            {
                currentService = DependencyService.Get<INotificationService>();
                currentService.start();
                currentService.tokenUpdated += OnNewToken;
                currentService.pushReceived += OnNewNotification;
            }
        }

        private static void OnNewToken(object sender, string NewToken)
        {
            int a = 3;
        }

        private static void OnNewNotification(object sender, IDictionary<string, string> data) {
            int b = 4;
        }

        public static async Task<string> GetTokenAsync()
        {
            INotificationService service = DependencyService.Get<INotificationService>();
            string token = await service.GetDeviceTokenAsync();
            return token;
        }
    }
}
