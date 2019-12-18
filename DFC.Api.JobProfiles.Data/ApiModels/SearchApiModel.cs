using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels
{
    public class SearchApiModel
    {
        [Example(Description = "List of search results")]
        public IEnumerable<SearchItemApiModel> Results { get; set; }

        [Example(Description = "10")]
        public long? Count { get; set; }
    }
}
