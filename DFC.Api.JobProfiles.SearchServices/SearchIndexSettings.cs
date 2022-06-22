using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.JobProfiles.SearchServices
{
    [ExcludeFromCodeCoverage]
    public class SearchIndexSettings
    {
        public string SearchIndex { get; set; }

        public string SearchServiceName { get; set; }

        public string AccessKey { get; set; }
    }
}
