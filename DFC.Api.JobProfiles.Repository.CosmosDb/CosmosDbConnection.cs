using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.JobProfiles.Repository.CosmosDb
{
    [ExcludeFromCodeCoverage]
    public class CosmosDbConnection
    {
        public string AccessKey { get; set; }

        public string EndpointUrl { get; set; }

        public string DatabaseId { get; set; }

        public string CollectionId { get; set; }

        public string PartitionKey { get; set; }
    }
}