using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;

using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

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
        private readonly ISearchIndexClient indexClient;

        public AzSearchQueryService(IAzSearchQueryConverter queryConverter, ISearchIndexClient indexClient)
        {
            this.queryConverter = queryConverter;
            this.indexClient = indexClient;
        }

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            return await GetCurrentStatusWithSearchIndexAsync(indexClient)
                .ConfigureAwait(false);
        }

        public virtual async Task<Data.AzureSearch.Models.SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties = null)
        {
            return await SearchWithSearchIndexClientAsync(indexClient, searchTerm, properties)
                .ConfigureAwait(false);
        }

        public async Task<SuggestionResult<T>> GetSuggestionAsync(string partialTerm, SuggestProperties properties)
        {
            return await GetSuggestionAsyncWithIndexClient(indexClient, partialTerm, properties)
                .ConfigureAwait(false);
        }

        private async Task<ServiceStatus> GetCurrentStatusWithSearchIndexAsync(ISearchIndexClient searchIndexClient)
        {
            const string searchTerm = "*";

            var serviceStatus = new ServiceStatus
            {
                Name = ServiceName,
                Status = ServiceState.Red,
                Notes = string.Empty,
                CheckParametersUsed = $"Search term - {searchTerm}",
            };

            var searchParam = new SearchParameters { Top = 5 };
            var result = await searchIndexClient.Documents.SearchAsync<T>(searchTerm, searchParam).ConfigureAwait(false);

            // The call worked ok
            serviceStatus.Status = ServiceState.Amber;
            serviceStatus.Notes = "Success search with 0 results";

            if (result.Results.Count <= 0)
            {
                return serviceStatus;
            }

            serviceStatus.Status = ServiceState.Green;
            serviceStatus.Notes = string.Empty;

            return serviceStatus;
        }

        private async Task<Data.AzureSearch.Models.SearchResult<T>> SearchWithSearchIndexClientAsync(ISearchIndexClient searchIndexClient, string searchTerm, SearchProperties properties = null)
        {
            var searchParam = queryConverter.BuildSearchParameters(properties);
            var result = await searchIndexClient.Documents.SearchAsync<T>(searchTerm, searchParam).ConfigureAwait(false);
            var output = queryConverter.ConvertToSearchResult(result, properties);
            output.ComputedSearchTerm = searchTerm;
            output.SearchParametersQueryString = searchParam.ToString();
            return output;
        }

        private async Task<SuggestionResult<T>> GetSuggestionAsyncWithIndexClient(ISearchIndexClient searchIndexClient, string partialTerm, SuggestProperties properties)
        {
            var suggestParameters = queryConverter.BuildSuggestParameters(properties);
            var result = await searchIndexClient.Documents.SuggestAsync<T>(partialTerm, DefaultSuggesterName, suggestParameters).ConfigureAwait(false);
            return queryConverter.ConvertToSuggestionResult(result, properties);
        }
    }
}