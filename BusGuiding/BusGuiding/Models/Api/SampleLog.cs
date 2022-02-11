using BusGuiding.Models.Api.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusGuiding.Models.Api
{
    public class SampleLog : BasicRest
    {

        public static async Task<Dictionary<string, string>> AddSampleLog(string apiToken, string sampleType, DateTime deliveryTime)
        {
            Dictionary<string, string> data = new Dictionary<string, string>() { { "sample_date", deliveryTime.ToString("yyyy-MM-dd HH:mm:ss") }, { "sample_type", sampleType } };
            return await ExecuteRequest<Dictionary<string, string>>(Constants.Api.SampleLog, Method.POST, apiToken, data);
        }

    }
}
