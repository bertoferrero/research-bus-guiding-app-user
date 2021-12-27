
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
        public event EventHandler<string> TokenUpdated;
        public event EventHandler<IDictionary<string, string>> NewNotification;

        #region singleton
        private static NotificationHandler instance;
        public static NotificationHandler Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new NotificationHandler();
                    instance.initEvents();
                }
                return instance;
            }
        }

        private NotificationHandler()
        {

        }

        #endregion
        public void initEvents()
        {
            if (currentService == null)
            {
                currentService = DependencyService.Get<INotificationService>();
                currentService.start();
                currentService.tokenUpdated += OnNewToken;
                currentService.pushReceived += OnNewNotification;
            }
        }

        private void OnNewToken(object sender, string NewToken) => TokenUpdated(this, NewToken);

        private void OnNewNotification(object sender, IDictionary<string, string> data) => NewNotification(this, data);

        public async Task<string> GetTokenAsync() => await currentService.GetDeviceTokenAsync();
    }
}
