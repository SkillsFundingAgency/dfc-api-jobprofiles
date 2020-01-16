using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.Api.JobProfiles.IntegrationTests.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    internal class CommonAction
    {
        private static Random Random = new Random();

        internal static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        internal static void InitialiseAppSettings()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            Settings.ServiceBusConfig.Endpoint = Configuration.GetSection("ServiceBusConfig").GetSection("Endpoint").Value;
            Settings.APIConfig.Version = Configuration.GetSection("APIConfig").GetSection("Version").Value;
            Settings.APIConfig.ApimSubscriptionKey = Configuration.GetSection("APIConfig").GetSection("ApimSubscriptionKey").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileDetail = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileDetail").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileSearch = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileSearch").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileSummary = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileSummary").Value;
            if (!int.TryParse(Configuration.GetSection("GracePeriodInSeconds").Value, out int gracePeriodInSeconds)) { throw new InvalidCastException("Unable to retrieve an integer value for the grace period setting"); }
            Settings.GracePeriod = TimeSpan.FromSeconds(gracePeriodInSeconds);
        }

        private static byte[] ConvertObjectToByteArray(object obj)
        {
            string serialisedContent = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(serialisedContent);
        }

        private static Message CreateCreateMessage(Guid messageId, byte[] messageBody)
        {
            Message message = new Message();
            message.ContentType = "application/json";
            message.Body = messageBody;
            message.CorrelationId = Guid.NewGuid().ToString();
            message.Label = "Automated message";
            message.MessageId = Guid.NewGuid().ToString();
            message.UserProperties.Add("Id", messageId);
            message.UserProperties.Add("ActionType", "Published");
            message.UserProperties.Add("CType", "JobProfile");
            return message;
        }

        private static Message CreateDeleteMessage(Guid messageId, byte[] messageBody)
        {
            Message message = new Message();
            message.ContentType = "application/json";
            message.UserProperties.Add("Id", messageId);
            message.UserProperties.Add("ActionType", "Deleted");
            message.UserProperties.Add("CType", "JobProfile");
            message.Label = "Automated message";
            message.Body = messageBody;
            return message;
        }

        internal async static Task DeleteJobProfileWithId(Topic topic, Guid jobProfileId)
        {
            JobProfileDeleteMessageBody messageBody = ResourceManager.GetResource<JobProfileDeleteMessageBody>("JobProfileDeleteMessageBody");
            messageBody.JobProfileId = jobProfileId.ToString();
            Message deleteMessage = CommonAction.CreateDeleteMessage(jobProfileId, CommonAction.ConvertObjectToByteArray(messageBody));
            await topic.SendAsync(deleteMessage);
        }

        internal async static Task CreateJobProfile(Topic topic, Guid messageId, string canonicalName)
        {
            JobProfileCreateMessageBody messageBody = ResourceManager.GetResource<JobProfileCreateMessageBody>("JobProfileCreateMessageBody");
            messageBody.JobProfileId = messageId.ToString();
            messageBody.UrlName = canonicalName;
            messageBody.CanonicalName = canonicalName;
            Message message = CreateCreateMessage(messageId, CommonAction.ConvertObjectToByteArray(messageBody));
            await topic.SendAsync(message);
        }

        internal async static Task<Response<T>> ExecuteGetRequest<T>(string endpoint, bool AuthoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint);
            getRequest.AddVersionHeader(Settings.APIConfig.Version);

            if(AuthoriseRequest)
            {
                getRequest.AddApimKeyHeader(Settings.APIConfig.ApimSubscriptionKey);
            } else
            {
                getRequest.AddApimKeyHeader(RandomString(20).ToLower());
            }

            await Task.Delay(3000); //This needs removing once DFC-11492 has been fixed.
            Response<T> response = getRequest.Execute<T>();

            DateTime startTime = DateTime.Now;
            while(response.HttpStatusCode.Equals(HttpStatusCode.NoContent) && DateTime.Now - startTime < Settings.GracePeriod)
            {
                await Task.Delay(500);
                response = getRequest.Execute<T>();
            }

            return response;
        }
    }
}
