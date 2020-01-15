using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.Api.JobProfiles.IntegrationTests.Model;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    public class Hook
    {
        public Guid MessageId { get; set; }
        public string CanonicalName { get; set; }
        public JobProfileMessageBody MessageBody { get; set; }
        public List<Guid> DeletionQueue { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            DeletionQueue = new List<Guid>();
            IConfigurationRoot Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            Settings.ServiceBusConfig.Endpoint = Configuration.GetSection("ServiceBusConfig").GetSection("Endpoint").Value;
            Settings.APIConfig.Version = Configuration.GetSection("APIConfig").GetSection("Version").Value;
            Settings.APIConfig.ApimSubscriptionKey = Configuration.GetSection("APIConfig").GetSection("ApimSubscriptionKey").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileDetail = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileDetail").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileSearch = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileSearch").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileSummary = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileSummary").Value;
            if (!int.TryParse(Configuration.GetSection("GracePeriodInSeconds").Value, out int gracePeriodInSeconds)) { throw new InvalidCastException("Unable to retrieve an integer value for the grace period setting"); }
            Settings.GracePeriod = TimeSpan.FromSeconds(gracePeriodInSeconds);

            MessageId = Guid.NewGuid();
            CanonicalName = CommonAction.RandomString(10).ToLower();

            MessageBody = ResourceManager.GetResource<JobProfileMessageBody>("JobProfileMessageBody");
            MessageBody.JobProfileId = MessageId.ToString();
            MessageBody.UrlName = CanonicalName;
            MessageBody.CanonicalName = CanonicalName;

            Topic topic = new Topic(Settings.ServiceBusConfig.Endpoint);
            Message message = CommonAction.CreateMessage(MessageId, CommonAction.ConvertObjectToByteArray(MessageBody));
            await topic.SendAsync(message);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //Delete message with message ID.
        }
    }
}
