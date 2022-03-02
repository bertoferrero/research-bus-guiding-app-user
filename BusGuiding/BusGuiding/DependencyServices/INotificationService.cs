using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.DependencyServices
{
    public interface INotificationService
    {
        event EventHandler<string> tokenUpdated;
        event EventHandler<IDictionary<string, string>> pushReceived;

        void start();

        Task<string> GetDeviceTokenAsync();

        public int ShowNotification(string title, string message, bool highImportance = false);

    }
}
