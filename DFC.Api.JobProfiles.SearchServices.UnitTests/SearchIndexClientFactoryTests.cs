using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.Settings;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using Dfc.SharedConfig.Services;
using FakeItEasy;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests
{
    public class SearchIndexClientFactoryTests
    {
        private readonly JobProfileSearchIndexConfig defaultSharedConfig;
        private readonly ISharedConfigurationService defaultSharedConfigService;
        private readonly ISearchIndexClientFactory defaultConfigFactory;
        private readonly SharedConfigParameters defaultConfigParams;

        public SearchIndexClientFactoryTests()
        {
            defaultSharedConfig = new JobProfileSearchIndexConfig { SearchIndex = "SharedIndexName", AccessKey = "SharedAccessKey", SearchServiceName = "JobProfilesApiTest" };
            defaultSharedConfigService = A.Fake<ISharedConfigurationService>();
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).Returns(defaultSharedConfig);

            defaultConfigParams = new SharedConfigParameters { SharedConfigServiceName = "TestServiceName", SharedConfigKeyName = "TestKeyName" };
            defaultConfigFactory = new SearchIndexClientFactory(defaultSharedConfigService, defaultConfigParams);
        }

        [Fact]
        public async Task GetSearchIndexClientWhenIndexClientIsNullCallSharedConfigurationService()
        {
            // Act
            var result = await defaultConfigFactory.GetSearchIndexClient().ConfigureAwait(false);

            // Assert
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(defaultSharedConfig.SearchIndex, result.IndexName);
        }

        [Fact]
        public async Task GetSearchIndexClientRefreshesIndexClientFromSharedConfig()
        {
            // Arrange
            var staleSharedConfig = new JobProfileSearchIndexConfig { SearchIndex = "StaleIndexName", AccessKey = "SharedAccessKey", SearchServiceName = "JobProfilesApiTest" };
            var sharedConfigService = A.Fake<ISharedConfigurationService>();
            A.CallTo(() => sharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).Returns(staleSharedConfig);
            var searchIndexClientFactory = new SearchIndexClientFactory(sharedConfigService, defaultConfigParams);

            // Act
            var staleIndex = await searchIndexClientFactory.GetSearchIndexClient().ConfigureAwait(false);

            A.CallTo(() => sharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).Returns(defaultSharedConfig);
            var freshIndex = await searchIndexClientFactory.GetSearchIndexClient().ConfigureAwait(false);

            // Assert
            Assert.NotEqual(staleIndex.IndexName, freshIndex.IndexName);
            Assert.Equal(defaultSharedConfig.SearchIndex, freshIndex.IndexName);
            A.CallTo(() => sharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task CreateOrRefreshIndexClientCallSharedConfigurationService()
        {
            // Act
            var result = await defaultConfigFactory.CreateOrRefreshIndexClient().ConfigureAwait(false);

            // Assert
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(defaultSharedConfig.SearchIndex, result.IndexName);
        }
    }
}