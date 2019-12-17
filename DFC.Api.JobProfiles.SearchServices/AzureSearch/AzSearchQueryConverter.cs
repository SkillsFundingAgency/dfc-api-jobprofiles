using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Extensions;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using Microsoft.Azure.Search.Models;
using System;

namespace DFC.Api.JobProfiles.SearchServices.AzureSearch
{
    public class AzSearchQueryConverter : IAzSearchQueryConverter
    {
        public SearchParameters BuildSearchParameters(SearchProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            return new SearchParameters
            {
                SearchMode = SearchMode.Any,
                IncludeTotalResultCount = true,
                SearchFields = properties.SearchFields,
                Filter = properties.FilterBy,
                Skip = (properties.Page - 1) * properties.Count,
                Top = properties.Count,
                QueryType = QueryType.Full,
                OrderBy = properties.OrderByFields,
                ScoringProfile = properties.ScoringProfile,
            };
        }

        public SuggestParameters BuildSuggestParameters(SuggestProperties properties)
        {
            return new SuggestParameters
            {
                UseFuzzyMatching = properties?.UseFuzzyMatching ?? true,
                Top = properties?.MaxResultCount,
            };
        }

        public Data.AzureSearch.Models.SearchResult<T> ConvertToSearchResult<T>(DocumentSearchResult<T> result, SearchProperties properties)
            where T : class
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new Data.AzureSearch.Models.SearchResult<T>
            {
                Count = result.Count,
                Results = result.ToSearchResultItems(properties),
                Coverage = result.Coverage,
            };
        }

        public SuggestionResult<T> ConvertToSuggestionResult<T>(DocumentSuggestResult<T> result, SuggestProperties properties)
            where T : class
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new SuggestionResult<T>
            {
                Coverage = result.Coverage,
                Results = result.ToSuggestResultItems(),
            };
        }
    }
}