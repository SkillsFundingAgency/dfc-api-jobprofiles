using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class PreSearchFiltersResultsModel
    {
        public ICollection<FilterResultsSection> Sections { get; set; }
    }
}