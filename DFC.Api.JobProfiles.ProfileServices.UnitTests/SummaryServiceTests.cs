using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.ProfileServices.UnitTests
{
    public class SummaryServiceTests
    {
        private const string RequestUrl = "http://Something.com/";
        private readonly IMapper mapper;
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;

        public SummaryServiceTests()
        {
            mapper = A.Fake<IMapper>();
            sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();
            var apiModels = GetSummaryApiModels();
            A.CallTo(() => mapper.Map<List<SummaryApiModel>>(A<List<SummaryDataModel>>.Ignored)).Returns(apiModels);
        }

        [Fact]
        public async Task GetSummaryListReturnsApiModelsWithCorrectUrlPrefixWhenDataExists()
        {
            // Arrange
            var dataModels = GetSummaryDataModels();

            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileApiSummaryResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(dataModels);

            var summaryService = new SummaryService(mapper, sharedContentRedisInterface);

            // Act
            var result = await summaryService.GetSummaryList(RequestUrl).ConfigureAwait(false);

            // Assert
            var resultArray = result.ToArray();

            for (var i = 0; i < resultArray.Length; i++)
            {
                Assert.Equal($"{RequestUrl}{dataModels.JobProfileSummary[i].DisplayText}", resultArray[i].Url);
            }
        }

        [Fact]
        public async Task GetSummaryListReturnsNullWhenDataDoesntExist()
        {
            // Arrange
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileApiSummaryResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(new JobProfileApiSummaryResponse());


            var summaryService = new SummaryService(mapper, sharedContentRedisInterface);

            // Act
            var result = await summaryService.GetSummaryList(RequestUrl).ConfigureAwait(false);

            // Assert
            Assert.Null(result);
        }

        private static JobProfileApiSummaryResponse GetSummaryDataModels()
        {
            var expectedResult = new JobProfileApiSummaryResponse();

            var list = new List<JobProfileSummary>
            {
                new JobProfileSummary
                {
                   DisplayText = "Test1",
                   PageLocation = new ()
                    {
                        UrlName = "TestUrlName",
                        FullUrl = "TestFullUrl1",
                    },
                   PublishedUtc = "1992-01-01",
                },
                new JobProfileSummary
                {
                   DisplayText = "Test2",
                   PageLocation = new ()
                    {
                        UrlName = "TestUrlName",
                        FullUrl = "TestFullUrl2",
                    },
                   PublishedUtc = "1992-01-01",
                },
            };
            expectedResult.JobProfileSummary = list;
            return expectedResult;
        }

        private List<SummaryApiModel> GetSummaryApiModels()
        {
            return new List<SummaryApiModel>
            {
                new SummaryApiModel
                {
                    Title = "JobTitle1",
                    Url = "job1",
                    LastUpdated = DateTime.UtcNow,
                },
                new SummaryApiModel
                {
                    Title = "JobTitle2",
                    Url = "job2",
                    LastUpdated = DateTime.UtcNow.AddDays(-1),
                },
            };
        }
    }
}