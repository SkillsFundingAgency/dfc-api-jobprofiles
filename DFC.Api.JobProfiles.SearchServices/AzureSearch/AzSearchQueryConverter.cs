using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Extensions;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using System;

namespace DFC.Api.JobProfiles.SearchServices.AzureSearch
{
    public class AzSearchQueryConverter : IAzSearchQueryConverter
    {
        public SearchOptions BuildSearchParameters(SearchProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var searchOptions = new SearchOptions
            {
                SearchMode = SearchMode.Any,
                IncludeTotalCount = true,
                Filter = properties.FilterBy,
                Skip = (properties.Page - 1) * properties.Count,
                Size = properties.Count,
                QueryType = SearchQueryType.Full,
                ScoringProfile = properties.ScoringProfile,
            };

            if (properties.OrderByFields != null)
            {
                foreach (var field in properties.OrderByFields)
                {
                    searchOptions.OrderBy.Add(field);
                }
            }

            if (properties.SearchFields != null)
            {
                foreach (var field in properties.SearchFields)
                {
                    searchOptions.SearchFields.Add(field);
                }
            }

            return searchOptions;
        }

        public SuggestOptions BuildSuggestParameters(SuggestProperties properties)
        {
            return new SuggestOptions
            {
                UseFuzzyMatching = properties?.UseFuzzyMatching ?? true,
                Size = properties?.MaxResultCount,
            };
        }

        public Data.AzureSearch.Models.SearchResult<T> ConvertToSearchResult<T>(SearchResults<T> results, SearchProperties properties)
            where T : class
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            return new Data.AzureSearch.Models.SearchResult<T>
            {
                Count = results.TotalCount,
                Results = results.ToSearchResultItems(properties),
                Coverage = results.Coverage,
            };
        }

        public SuggestionResult<T> ConvertToSuggestionResult<T>(SuggestResults<T> result, SuggestProperties properties)
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