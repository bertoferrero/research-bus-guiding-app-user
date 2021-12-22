using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class User
    {
        public static async Task LoginAsync(string user, string password)
        {
            //https://www.luisllamas.es/consumir-un-api-rest-en-c-facilmente-con-restsharp/
            var client = new RestClient(Constants.Api.ApiEndpoint);
            var request = new RestRequest(Constants.Api.LoginPath, Method.POST);
            var authData = new Dictionary<string, string> { { "username", user }, { "password", password } };
            //var authData = new BasicAuth { username = user, password = password };
            request.AddJsonBody(authData);
            var response = await client.ExecuteAsync(request);
            CheckResponse(response);
            var responseContent = client.Deserialize<Dictionary<string, string>>(response);
            Console.WriteLine(response.Content);
        }

        
        protected static void CheckResponse(IRestResponse response)
        {
            //Check errors
            if(!response.ResponseStatus.Equals(ResponseStatus.Completed))
            {
                throw new ConnectionException(response.ErrorMessage, response.ErrorException, response.ResponseStatus);
            }
            //Check code
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                throw new StatusCodeException(response.StatusDescription, response.ErrorException, response.StatusCode);
            }
        }
    }

    class BasicAuth
    {
        public string username;
        public string password;
    }
}
