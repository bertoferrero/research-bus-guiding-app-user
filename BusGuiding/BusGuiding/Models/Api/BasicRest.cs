using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class BasicRest
    {
        /// <summary>
        /// Executes the Rest request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        protected static async Task<T> ExecuteRequest<T>(string resource, Method method, string? authToken = null, object? jsonBody = null)
        {
            var request = new RestRequest(resource, method);
            if (!string.IsNullOrEmpty(authToken))
            {
                request.AddHeader("X-AUTH-TOKEN", authToken);
            }
            if(jsonBody != null)
            {
                request.AddJsonBody(jsonBody);
            }
            return await ExecuteRequest<T>(request);
        }

        /// <summary>
        /// Executes the Rest request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        protected static async Task<T> ExecuteRequest<T>(RestRequest request)
        {
            var client = new RestClient(Constants.Api.ApiEndpoint);
            var response = await client.ExecuteAsync(request);
            CheckResponse(response);
            T responseContent = client.Deserialize<T>(response).Data;
            return responseContent;
        }


        protected static void CheckResponse(IRestResponse response)
        {
            //Check errors
            if (!response.ResponseStatus.Equals(ResponseStatus.Completed))
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
}
