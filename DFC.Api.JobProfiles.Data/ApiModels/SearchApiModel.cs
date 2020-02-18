using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels
{
    public class SearchApiModel
    {
        [Example(Description = "42")]
        public long? Count { get; set; }

        [Example(Description = "2")]
        public long? CurrentPage { get; set; }

        [Example(Description = "10")]
        public long? PageSize { get; set; }

        [Example(Description = "List of search results")]
        public IEnumerable<SearchItemApiModel> Results { get; set; }
    }
}
