using Azure.Search.Documents.Models;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Api.JobProfiles.SearchServices.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SearchExtensions
    {
        public static IEnumerable<SearchResultItem<T>> ToSearchResultItems<T>(this SearchResults<T> results, SearchProperties properties)
        where T : class
        {
            if (properties != null && results != null)
            {
                var beginRank = ((properties.Page - 1) * properties.Count) + properties.ExactMatchCount;

                var resultList = new List<SearchResultItem<T>>();

                if (results.GetResults() != null)
                {
                    foreach (var result in results.GetResults())
                    {
                        beginRank++;
                        resultList.Add(new SearchResultItem<T>
                        {
                            ResultItem = result.Document,
                            Rank = beginRank,
                            Score = (double)result.Score,
                        });
                    }
                }

                return resultList;
            }

            return Enumerable.Empty<SearchResultItem<T>>();
        }

        public static IEnumerable<SuggestionResultItem<T>> ToSuggestResultItems<T>(this SuggestResults<T> results)
            where T : class
        {
            return results?.Results?.Select(r => new SuggestionResultItem<T>
            {
                Index = r.Document,
                MatchedSuggestion = r.Text,
            });
        }
    }
}