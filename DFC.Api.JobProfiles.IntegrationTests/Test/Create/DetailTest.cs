using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.Api.JobProfiles.IntegrationTests.Model;
using DFC.Api.JobProfiles.IntegrationTests.Support;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Test.Create
{
    public class DetailTest : Hook
    {
        public Guid MessageId { get; set; }
        public JobProfileInputModel MessageBody { get; set; }
        public Response<JobProfileDetails> JobProfileDetailsResponse { get; set; }

        [SetUp]
        public async Task Setup()
        {
            MessageId = Guid.NewGuid();
            string canonicalName = CommonAction.RandomString(10).ToLower();

            MessageBody = ResourceManager.GetResource<JobProfileInputModel>("MessageBody");
            MessageBody.JobProfileId = MessageId.ToString();
            MessageBody.UrlName = canonicalName;
            MessageBody.CanonicalName = canonicalName;

            Topic topic = new Topic(Settings.ServiceBusConfig.Endpoint);
            Message message = CommonAction.CreateMessage(MessageId, CommonAction.ConvertObjectToByteArray(MessageBody));
            await topic.SendAsync(message);

            JobProfileDetailsResponse = await CommonAction.ExecuteGetRequest<JobProfileDetails>(Settings.APIConfig.EndpointBaseUrl.ProfileDetail, canonicalName);
        }

        [TearDown]
        public void TearDown()
        {
            DeletionQueue.Add(MessageId);
        }

        [Test]
        public void ResponseCode200()
        {
            Assert.AreEqual(HttpStatusCode.OK, JobProfileDetailsResponse.HttpStatusCode);
        }
    }
}