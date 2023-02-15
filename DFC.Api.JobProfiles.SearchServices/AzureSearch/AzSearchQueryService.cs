using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices.AzureSearch
{
    [ExcludeFromCodeCoverage]
    public class AzSearchQueryService<T> : ISearchQueryService<T>, IServiceStatus
        where T : class
    {
        private const string DefaultSuggesterName = "sg";
        private const string ServiceName = "Search Service";

        private readonly IAzSearchQueryConverter queryConverter;
        private readonly SearchClient searchClient;

        public AzSearchQueryService(IAzSearchQueryConverter queryConverter, SearchClient searchClient)
        {
            this.queryConverter = queryConverter;
            this.searchClient = searchClient;
        }

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            return await GetCurrentStatusWithSearchIndexAsync(searchClient)
                .ConfigureAwait(false);
        }

        public virtual async Task<Data.AzureSearch.Models.SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties = null)
        {
            return await SearchWithSearchIndexClientAsync(searchClient, searchTerm, properties)
                .ConfigureAwait(false);
        }

        public async Task<SuggestionResult<T>> GetSuggestionAsync(string partialTerm, SuggestProperties properties)
        {
            return await GetSuggestionAsyncWithIndexClient(searchClient, partialTerm, properties)
                .ConfigureAwait(false);
        }

        private async Task<ServiceStatus> GetCurrentStatusWithSearchIndexAsync(SearchClient searchClient)
        {
            const string searchTerm = "*";

            var serviceStatus = new ServiceStatus
            {
                Name = ServiceName,
                Status = ServiceState.Red,
                Notes = string.Empty,
                CheckParametersUsed = $"Search term - {searchTerm}",
            };

            var searchParam = new SearchOptions { Size = 5 };
            var result = await searchClient.SearchAsync<T>(searchTerm, searchParam).ConfigureAwait(false);

            // The call worked ok
            serviceStatus.Status = ServiceState.Amber;
            serviceStatus.Notes = "Success search with 0 results";

            if (result.Value.TotalCount <= 0)
            {
                return serviceStatus;
            }

            serviceStatus.Status = ServiceState.Green;
            serviceStatus.Notes = string.Empty;

            return serviceStatus;
        }

        private async Task<Data.AzureSearch.Models.SearchResult<T>> SearchWithSearchIndexClientAsync(SearchClient searchClient, string searchTerm, SearchProperties properties = null)
        {
            var searchParam = queryConverter.BuildSearchParameters(properties);
            var result = await searchClient.SearchAsync<T>(searchTerm, searchParam).ConfigureAwait(false);
            var output = queryConverter.ConvertToSearchResult(result.Value, properties);
            output.ComputedSearchTerm = searchTerm;
            output.SearchParametersQueryString = searchParam.ToString();
            return output;
        }

        private async Task<SuggestionResult<T>> GetSuggestionAsyncWithIndexClient(SearchClient searchClient, string partialTerm, SuggestProperties properties)
        {
            var suggestParameters = queryConverter.BuildSuggestParameters(properties);
            var result = await searchClient.SuggestAsync<T>(partialTerm, DefaultSuggesterName, suggestParameters).ConfigureAwait(false);
            return queryConverter.ConvertToSuggestionResult(result.Value, properties);
        }
    }
}