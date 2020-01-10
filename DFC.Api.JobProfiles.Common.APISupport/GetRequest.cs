using RestSharp;

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

        public IRestResponse<T> Execute<T>()
        {
            RestClient restClient = new RestClient();
            return restClient.Execute<T>(Request);
        }
    }
}
