using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.DependencyServices
{
    public interface INotificationService
    {
        Task<string> GetDeviceTokenAsync();
    }
}
