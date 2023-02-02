using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface IAzSearchQueryConverter
    {
        SearchOptions BuildSearchParameters(SearchProperties properties);

        Data.AzureSearch.Models.SearchResult<T> ConvertToSearchResult<T>(Azure.Search.Documents.Models.SearchResults<T> result, SearchProperties properties)
            where T : class;

        SuggestOptions BuildSuggestParameters(SuggestProperties properties);

        SuggestionResult<T> ConvertToSuggestionResult<T>(SuggestResults<T> result, SuggestProperties properties)
            where T : class;
    }
}