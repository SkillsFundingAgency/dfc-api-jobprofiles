using DFC.Api.JobProfiles.Data.AzureSearch.Enums;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Api.JobProfiles.SearchServices
{
    public class DfcBuildFilterService : IBuildSearchFilterService
    {
        public string BuildPreSearchFilters(PreSearchFiltersResultsModel preSearchFilterModel, IDictionary<string, PreSearchFilterLogicalOperator> indexFields)
        {
            var builder = new System.Text.StringBuilder();
            if (indexFields != null)
            {
                foreach (var field in indexFields)
                {
                    var validIndexField = typeof(JobProfileIndex).GetProperties()
                        .Any(property => property.Name.Equals(field.Key));

                    if (validIndexField)
                    {
                        var fieldFilter = preSearchFilterModel?.Sections?.FirstOrDefault(section =>
                            section.SectionDataTypes.Equals(field.Key, StringComparison.InvariantCultureIgnoreCase));
                        if (fieldFilter != null)
                        {
                            BuildPreSearchFiltersForField(builder, fieldFilter, field);
                        }
                    }
                }
            }

            return builder.ToString().Trim();
        }

        private void BuildPreSearchFiltersForField(System.Text.StringBuilder builder, FilterResultsSection fieldFilter, KeyValuePair<string, PreSearchFilterLogicalOperator> field)
        {
            var notApplicableSelected = fieldFilter.Options.Any(opt => opt.ClearOtherOptionsIfSelected);
            if (!notApplicableSelected)
            {
                var fieldValue = fieldFilter.SingleSelectOnly
                    ? fieldFilter.SingleSelectedValue
                    : string.Join(",", fieldFilter.Options.Where(opt => opt.IsSelected).Select(opt => opt.OptionKey));
                if (!string.IsNullOrWhiteSpace(fieldValue))
                {
                    builder.Append($"{(builder.Length > 0 ? field.Value.ToString().ToLowerInvariant() : string.Empty)} {field.Key}/any(t: search.in(t, '{fieldValue}')) ");
                }
            }
        }
    }
}