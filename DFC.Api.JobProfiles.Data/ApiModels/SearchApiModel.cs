using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels
{
    public class SearchApiModel<T>
           where T : class
    {
        [Example(Description = "List of search results")]
        public IEnumerable<T> Results { get; set; }

        [Example(Description = "10")]
        public long? Count { get; set; }
    }
}
