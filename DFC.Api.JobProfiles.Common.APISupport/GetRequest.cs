using Newtonsoft.Json;
using RestSharp;
using System;

namespace DFC.Api.JobProfiles.Common.APISupport
{
    public class GetRequest
    {
        private RestRequest Request { get; set; }

        public GetRequest(string endpoint)
        {
            Request = new RestRequest(endpoint, Method.GET);
            Request.AddHeader("Content-Type", "application/json");
        }

        public void AddQueryParameter(string name, string value)
        {
            Request.AddParameter(name, value);
        }

        public void AddVersionHeader(string version)
        {
            Request.AddHeader("version", version);
        }

        public void AddApimKeyHeader(string apimSubscriptionKey)
        {
            Request.AddHeader("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
        }

        public Response<T> Execute<T>()
        {
            Response<T> response = new Response<T>();
            IRestResponse rawResponse = null;
            RestClient restClient = new RestClient();
            restClient.ExecuteAsync(Request, (response) => { rawResponse = response; });
            DateTime start = DateTime.Now;
            while (DateTime.Now - start < TimeSpan.FromSeconds(10) && rawResponse == null) { }
            
            if(rawResponse == null)
            {
                throw new TimeoutException("Unable to get a valid response from the API");
            }

            response.HttpStatusCode = rawResponse.StatusCode;
            response.IsSuccessful = rawResponse.IsSuccessful;
            response.ErrorMessage = rawResponse.ErrorMessage;
            response.ResponseStatus = rawResponse.ResponseStatus;
            response.Data = JsonConvert.DeserializeObject<T>(rawResponse.Content);

            return response;
        }
    }
}
