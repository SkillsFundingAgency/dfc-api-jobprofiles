using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    public class Hook
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            Settings.ServiceBusConfig.Endpoint = Configuration.GetSection("ServiceBusConfig").GetSection("Endpoint").Value;
            Settings.APIConfig.Version = Configuration.GetSection("APIConfig").GetSection("Version").Value;
            Settings.APIConfig.ApimSubscriptionKey = Configuration.GetSection("APIConfig").GetSection("ApimSubscriptionKey").Value;
        }
    }
}
