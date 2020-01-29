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

        public SearchIndexClientFactoryTests()
        {
            defaultSharedConfig = new JobProfileSearchIndexConfig { SearchIndex = "SharedIndexName", AccessKey = "SharedAccessKey", SearchServiceName = "JobProfilesApiTest" };

            defaultSharedConfigService = A.Fake<ISharedConfigurationService>();
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).Returns(defaultSharedConfig);
            var configParams = new SharedConfigParameters { SharedConfigServiceName = "TestServiceName", SharedConfigKeyName = "TestKeyName" };

            defaultConfigFactory = new SearchIndexClientFactory(defaultSharedConfigService, configParams);
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
        public async Task GetSearchIndexClientWhenIndexClientHasBeenRetrievedThenSharedConfigurationServiceNotCalledAgain()
        {
            // Act
            await defaultConfigFactory.GetSearchIndexClient().ConfigureAwait(false);
            var secondResult = await defaultConfigFactory.GetSearchIndexClient().ConfigureAwait(false);

            // Assert
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(defaultSharedConfig.SearchIndex, secondResult.IndexName);
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