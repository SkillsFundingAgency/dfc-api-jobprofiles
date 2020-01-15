using Dfc.SharedConfig.Services;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using Microsoft.Azure.Search;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices
{
    public class SharedConfigFactory : ISharedConfigFactory
    {
        private readonly JobProfileSearchIndexConfig config;
        private readonly ISharedConfigurationService service;
        private SearchIndexClient indexClient;

        public SharedConfigFactory(JobProfileSearchIndexConfig config, ISharedConfigurationService service)
        {
            this.config = config;
            this.service = service;
        }

        public async Task<ISearchIndexClient> GetSearchIndexClient()
        {
            if (string.IsNullOrWhiteSpace(config?.SearchIndex))//|| indexClient is null)
            {
                return await CreateClientFromPackage().ConfigureAwait(false);
            }
            else
            {
                if (indexClient is null)
                {
                    return new SearchIndexClient(config.SearchServiceName, config.SearchIndex, new SearchCredentials(config.AccessKey));
                }

                return indexClient;
            }
        }

        public async Task<ISearchIndexClient> CreateOrRefresh()
        {
            return await CreateClientFromPackage().ConfigureAwait(false);
        }

        private async Task<ISearchIndexClient> CreateClientFromPackage()
        {
            var configItem = await service.GetConfigAsync<JobProfileSearchIndexConfig>("JobProfilesApi", "JobProfileSearchIndexConfig").ConfigureAwait(false);
            indexClient = new SearchIndexClient(configItem.SearchServiceName, configItem.SearchIndex, new SearchCredentials(configItem.AccessKey));

            return indexClient;
        }
    }
}