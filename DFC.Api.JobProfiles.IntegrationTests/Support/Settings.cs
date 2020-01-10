namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    public class Settings
    {
        public class ServiceBusConfig
        {
            public static string Endpoint { get; set; }
        }

        public class APIConfig
        {
            public static string Version { get; set; }
            public static string ApimSubscriptionKey { get; set; }
        }
    }
}
