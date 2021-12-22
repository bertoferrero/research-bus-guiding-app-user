using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusGuiding.Models.Api.Exceptions
{
    class ConnectionException : Exception
    {
        public ResponseStatus Reason;

        public ConnectionException(string message, Exception innerException, ResponseStatus reason) : base(message, innerException)
        {
            this.Reason = reason;
        }
    }
}
