using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using Microsoft.Azure.Search.Models;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface IAzSearchQueryConverter
    {
        SearchParameters BuildSearchParameters(SearchProperties properties);

        Data.AzureSearch.Models.SearchResult<T> ConvertToSearchResult<T>(DocumentSearchResult<T> result, SearchProperties properties)
            where T : class;

        SuggestParameters BuildSuggestParameters(SuggestProperties properties);

        Data.AzureSearch.Models.SuggestionResult<T> ConvertToSuggestionResult<T>(DocumentSuggestResult<T> result, SuggestProperties properties)
            where T : class;
    }
}