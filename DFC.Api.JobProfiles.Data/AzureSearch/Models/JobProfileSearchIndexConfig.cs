namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class JobProfileSearchIndexConfig : ISearchIndexConfig
    {
        public string SearchIndex { get; set; }

        public string SearchServiceName { get; set; }

        public string AccessKey { get; set; }
    }
}