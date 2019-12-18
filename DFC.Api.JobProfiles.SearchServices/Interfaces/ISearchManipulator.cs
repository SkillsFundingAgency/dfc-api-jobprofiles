using DFC.Api.JobProfiles.Data.AzureSearch.Models;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface ISearchManipulator<T>
        where T : class
    {
        SearchResult<T> Reorder(SearchResult<T> searchResult, string searchTerm, SearchProperties searchProperties);

        string BuildSearchExpression(string searchTerm, string cleanedSearchTerm, string partialTermToSearch, SearchProperties properties);
    }
}