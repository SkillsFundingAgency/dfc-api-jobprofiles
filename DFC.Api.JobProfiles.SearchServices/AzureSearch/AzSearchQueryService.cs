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

        private readonly ISearchIndexClient indexClient;
        private readonly IAzSearchQueryConverter queryConverter;

        public AzSearchQueryService(ISearchIndexClient indexClient, IAzSearchQueryConverter queryConverter)
        {
            this.indexClient = indexClient;
            this.queryConverter = queryConverter;
        }

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };

            var searchTerm = "*";
            serviceStatus.CheckParametersUsed = $"Search term - {searchTerm}";

            var searchParam = new SearchParameters { Top = 5 };
            var result = await indexClient.Documents.SearchAsync<T>(searchTerm, searchParam).ConfigureAwait(false);

            // The call worked ok
            serviceStatus.Status = ServiceState.Amber;
            serviceStatus.Notes = "Success search with 0 results";

            if (result.Results.Count > 0)
            {
                serviceStatus.Status = ServiceState.Green;
                serviceStatus.Notes = string.Empty;
            }

            return serviceStatus;
        }

        public virtual async Task<Data.AzureSearch.Models.SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties = null)
        {
            var searchParam = queryConverter.BuildSearchParameters(properties);
            var result = await indexClient.Documents.SearchAsync<T>(searchTerm, searchParam).ConfigureAwait(false);
            var output = queryConverter.ConvertToSearchResult(result, properties);
            output.ComputedSearchTerm = searchTerm;
            output.SearchParametersQueryString = searchParam.ToString();
            return output;
        }

        public async Task<SuggestionResult<T>> GetSuggestionAsync(string partialTerm, SuggestProperties properties)
        {
            var suggestParameters = queryConverter.BuildSuggestParameters(properties);
            var result = await indexClient.Documents.SuggestAsync<T>(partialTerm, DefaultSuggesterName, suggestParameters).ConfigureAwait(false);
            return queryConverter.ConvertToSuggestionResult(result, properties);
        }
    }
}