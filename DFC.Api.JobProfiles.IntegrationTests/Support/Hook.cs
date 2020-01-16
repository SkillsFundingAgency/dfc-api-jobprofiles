using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    public class Hook
    {
        public Topic Topic { get; set; }
        public Guid MessageId { get; set; }
        public string CanonicalName { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            MessageId = Guid.NewGuid();
            CanonicalName = CommonAction.RandomString(10).ToLower();

            CommonAction.InitialiseAppSettings();
            Topic = new Topic(Settings.ServiceBusConfig.Endpoint);
            await CommonAction.CreateJobProfile(Topic, MessageId, CanonicalName);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await CommonAction.DeleteJobProfileWithId(Topic, MessageId);
        }
    }
}
