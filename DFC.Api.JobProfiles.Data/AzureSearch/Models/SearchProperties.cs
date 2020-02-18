using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class SearchProperties
    {
        public int Count { get; set; } = 10;

        public int Page { get; set; } = 1;

        public bool UseRawSearchTerm { get; set; }

        public IList<string> SearchFields { get; set; }

        public IList<string> OrderByFields { get; set; }

        public string FilterBy { get; set; }

        public int ExactMatchCount { get; set; }

        public string ScoringProfile { get; set; } = "jp";
    }
}