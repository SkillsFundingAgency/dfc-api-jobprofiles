using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using Dfc.SharedConfig.Services;
using FakeItEasy;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests
{
    public class SharedConfigFactoryTests
    {
        private readonly JobProfileSearchIndexConfig defaultLocalConfig;
        private readonly JobProfileSearchIndexConfig defaultSharedConfig;
        private readonly ISharedConfigurationService defaultSharedConfigService;
        private readonly ISharedConfigFactory defaultConfigFactory;

        public SharedConfigFactoryTests()
        {
            defaultLocalConfig = A.Fake<JobProfileSearchIndexConfig>();
            defaultSharedConfig = new JobProfileSearchIndexConfig { SearchIndex = "SharedIndexName", AccessKey = "SharedAccessKey", SearchServiceName = "JobProfilesApiTest" };

            defaultSharedConfigService = A.Fake<ISharedConfigurationService>();
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).Returns(defaultSharedConfig);

            defaultConfigFactory = new SharedConfigFactory(defaultLocalConfig, defaultSharedConfigService);
        }

        [Fact]
        public async Task GetSearchIndexClientWhenLocalConfigIsEmptyCallSharedConfigurationService()
        {
            // Act
            var result = await defaultConfigFactory.GetSearchIndexClient().ConfigureAwait(false);

            // Assert
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(defaultSharedConfig.SearchIndex, result.IndexName);
        }

        [Fact]
        public async Task GetSearchIndexClientWhenLocalConfigIsPresentUseLocalConfigToCreateIndexClient()
        {
            // Arrange
            var localConfig = new JobProfileSearchIndexConfig { SearchIndex = "LocalIndexName", AccessKey = "LocalAccessKey", SearchServiceName = "LocalJobProfilesApiTest" };
            var localFactory = new SharedConfigFactory(localConfig, defaultSharedConfigService);

            // Act
            var result = await localFactory.GetSearchIndexClient().ConfigureAwait(false);

            // Assert
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).MustNotHaveHappened();
            Assert.Equal(localConfig.SearchIndex, result.IndexName);
        }

        [Fact]
        public async Task GetSearchIndexClientWhenLocalConfigIsEmptyCallSharedConfigurationService2()
        {
            // Act
            var result = await defaultConfigFactory.CreateOrRefreshIndexClient().ConfigureAwait(false);

            // Assert
            A.CallTo(() => defaultSharedConfigService.GetConfigAsync<JobProfileSearchIndexConfig>(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(defaultSharedConfig.SearchIndex, result.IndexName);
        }
    }
}