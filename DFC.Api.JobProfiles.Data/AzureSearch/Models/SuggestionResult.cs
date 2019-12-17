using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class SuggestionResult<T>
        where T : class
    {
        public double? Coverage { get; set; }

        public IEnumerable<SuggestionResultItem<T>> Results { get; set; }
    }
}