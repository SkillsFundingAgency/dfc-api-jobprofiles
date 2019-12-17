using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface ISearchQueryService<T>
        where T : class
    {
        Task<SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties = null);

        Task<SuggestionResult<T>> GetSuggestionAsync(string partialTerm, SuggestProperties properties);
    }
}