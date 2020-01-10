using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.Api.JobProfiles.IntegrationTests.Model;
using DFC.Api.JobProfiles.IntegrationTests.Support;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests
{
    public class JobProfileDetailsTest : Hook
    {
        public JobProfileInputModel MessageBody { get; set; }
        public Response<JobProfileDetails> JobProfileDetailsResponse { get; set; }

        [SetUp]
        public async Task Setup()
        {
            string customId = Guid.NewGuid().ToString();
            string canonicalName = CommonAction.RandomString(10);
            MessageBody = ResourceManager.GetResource<JobProfileInputModel>("MessageBody");
            MessageBody.JobProfileId = customId;
            MessageBody.UrlName = canonicalName;
            MessageBody.CanonicalName = canonicalName;

            Topic topic = new Topic(Settings.ServiceBusConfig.Endpoint);
            Message message = new Message();
            message.ContentType = "application/json";
            message.Body = CommonAction.ConvertObjectToByteArray(MessageBody);
            message.CorrelationId = Guid.NewGuid().ToString();
            message.Label = "Automated message";
            message.MessageId = Guid.NewGuid().ToString();
            message.UserProperties.Add("Id", customId);
            message.UserProperties.Add("ActionType", "Published");
            message.UserProperties.Add("CType", "JobProfile");
            await topic.SendAsync(message);

            GetRequest getRequest = new GetRequest($"https://sit.api.nationalcareersservice.org.uk/job-profiles/{canonicalName}");
            getRequest.AddVersionHeader(Settings.APIConfig.Version);
            getRequest.AddApimKeyHeader(Settings.APIConfig.ApimSubscriptionKey);
            JobProfileDetailsResponse = getRequest.Execute<JobProfileDetails>();
        }

        [Test]
        public void Status200()
        {
            Assert.AreEqual(HttpStatusCode.OK, JobProfileDetailsResponse.HttpStatusCode);
        }

        [Test]
        public void TitleField()
        {
            Assert.AreEqual(MessageBody.Title, JobProfileDetailsResponse.Data.Title);
        }
    }
}