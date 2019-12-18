using DFC.Api.JobProfiles.Data.AzureSearch.Enums;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface IBuildSearchFilterService
    {
        string BuildPreSearchFilters(PreSearchFiltersResultsModel preSearchFilterModel, IDictionary<string, PreSearchFilterLogicalOperator> indexFields);
    }
}