using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BusGuiding.Models.Api.Exceptions
{
    class StatusCodeException : Exception
    {
        public HttpStatusCode StatusCode;

        public StatusCodeException(string message, Exception innerException, HttpStatusCode statusCode) : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }
    }
}
