using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.Api.JobProfiles.IntegrationTests.Model;
using DFC.Api.JobProfiles.IntegrationTests.Support;
using NUnit.Framework;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests
{
    public class APITests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task ServiceBusProofOfConcept()
        {
            string customId = Guid.NewGuid().ToString();
            string canonicalName = CommonAction.RandomString(10);

            //SET THE INPUT:
            Topic topic = new Topic("Endpoint=sb://dfc-sit-app-sharedresources-ns.servicebus.windows.net/;SharedAccessKeyName=monitor-cms-messages;SharedAccessKey=OoT1m7qwR9hwGOh/mNtQq/TVpETuKxjp/s5OE/8ZT1w=;EntityPath=cms-messages");
            Message message = new Message();
            message.Body = ResourceManager.GetResourceContent("MessageBody");
            message.ContentType = "application/json";
            message.CorrelationId = Guid.NewGuid().ToString();
            message.Label = "Automated message";
            message.MessageId = Guid.NewGuid().ToString();
            message.UserProperties.Add("Id", "685e06e6-a42d-4bec-b660-c206399c534j"); // needs changing
            message.UserProperties.Add("ActionType", "Published");
            message.UserProperties.Add("CType", "JobProfile");
            await topic.SendAsync(message);

            //READ THE OUTPUT
            GetRequest getRequest = new GetRequest("https://sit.api.nationalcareersservice.org.uk/job-profiles/abcdefg");
            getRequest.AddVersionHeader("v1");
            getRequest.AddApimKeyHeader("747a0f99d84c468da7f2f6e72003db53");
            IRestResponse<JobProfileDetails> jobProfileDetailsResponse = getRequest.Execute<JobProfileDetails>();
            JobProfileDetails jpd = jobProfileDetailsResponse.Data;

            Assert.AreEqual("Hello James", jpd.Title);
        }
    }
}