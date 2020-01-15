using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    public class Hook
    {
        public List<Guid> DeletionQueue { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
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
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            foreach(Guid id in DeletionQueue)
            {
                //Send a deletion request.
            }
        }
    }
}
