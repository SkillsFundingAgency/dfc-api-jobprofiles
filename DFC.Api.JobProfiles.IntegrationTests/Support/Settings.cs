using System;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    public class Settings
    {
        public static TimeSpan GracePeriod { get; set; }

        public class ServiceBusConfig
        {
            public static string Endpoint { get; set; }
        }

        public class APIConfig
        {
            public static string Version { get; set; }
            public static string ApimSubscriptionKey { get; set; }

            public class EndpointBaseUrl
            {
                public static string ProfileDetail { get; set; }
                public static string ProfileSearch { get; set; }
                public static string ProfileSummary { get; set; }
            }
        }
    }
}
