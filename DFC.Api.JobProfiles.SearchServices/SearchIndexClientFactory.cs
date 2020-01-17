using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.Settings;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using Dfc.SharedConfig.Services;
using Microsoft.Azure.Search;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices
{
    public class SearchIndexClientFactory : ISearchIndexClientFactory
    {
        private readonly ISharedConfigurationService sharedConfigurationService;
        private readonly SharedConfigParameters sharedConfigParameters;
        private SearchIndexClient indexClient;

        public SearchIndexClientFactory(ISharedConfigurationService sharedConfigurationService, SharedConfigParameters sharedConfigParameters)
        {
            this.sharedConfigurationService = sharedConfigurationService;
            this.sharedConfigParameters = sharedConfigParameters;
        }

        public async Task<ISearchIndexClient> GetSearchIndexClient()
        {
            if (indexClient is null)
            {
                await CreateSearchIndexClient().ConfigureAwait(false);
            }

            return indexClient;
        }

        public async Task<ISearchIndexClient> CreateOrRefreshIndexClient()
        {
            await CreateSearchIndexClient().ConfigureAwait(false);
            return indexClient;
        }

        private async Task CreateSearchIndexClient()
        {
            var configItem = await GetIndexConfig().ConfigureAwait(false);
            indexClient = new SearchIndexClient(configItem.SearchServiceName, configItem.SearchIndex, new SearchCredentials(configItem.AccessKey));
        }

        private async Task<JobProfileSearchIndexConfig> GetIndexConfig() => await sharedConfigurationService
            .GetConfigAsync<JobProfileSearchIndexConfig>(sharedConfigParameters.SharedConfigServiceName, sharedConfigParameters.SharedConfigKeyName)
            .ConfigureAwait(false);
    }
}