using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.ViewModels.Services
{
    public interface IMessageService
    {
        Task DisplayAlert(string title, string message, string cancel);
    }
}
