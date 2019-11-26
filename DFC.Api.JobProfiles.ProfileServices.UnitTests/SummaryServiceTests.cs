using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
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

        public SummaryServiceTests()
        {
            mapper = A.Fake<IMapper>();
            var apiModels = GetSummaryApiModels();
            A.CallTo(() => mapper.Map<IEnumerable<SummaryApiModel>>(A<IEnumerable<SummaryDataModel>>.Ignored)).Returns(apiModels);
        }

        [Fact]
        public async Task GetSummaryListReturnsApiModelsWithCorrectUrlPrefixWhenDataExists()
        {
            // Arrange
            var dataModels = GetSummaryDataModels();
            var repository = A.Fake<ICosmosRepository<SummaryDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SummaryDataModel, SummaryDataModel>>>.Ignored, null)).Returns(dataModels);

            var summaryService = new SummaryService(repository, mapper);

            // Act
            var result = await summaryService.GetSummaryList(RequestUrl).ConfigureAwait(false);

            // Assert
            var resultArray = result.ToArray();
            for (var i = 0; i < resultArray.Length; i++)
            {
                Assert.Equal($"{RequestUrl}{dataModels[i].CanonicalName}", resultArray[i].Url);
            }
        }

        [Fact]
        public async Task GetSummaryListReturnsNullWhenDataDoesntExist()
        {
            // Arrange
            var repository = A.Fake<ICosmosRepository<SummaryDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SummaryDataModel, SummaryDataModel>>>.Ignored, null)).Returns((IList<SummaryDataModel>)null);

            var summaryService = new SummaryService(repository, mapper);

            // Act
            var result = await summaryService.GetSummaryList(RequestUrl).ConfigureAwait(false);

            // Assert
            Assert.Null(result);
        }

        private IList<SummaryDataModel> GetSummaryDataModels()
        {
            return new List<SummaryDataModel>
            {
                new SummaryDataModel
                {
                    BreadcrumbTitle = "JobTitle1",
                    CanonicalName = "job1",
                    LastReviewed = DateTime.UtcNow,
                },
                new SummaryDataModel
                {
                    BreadcrumbTitle = "JobTitle2",
                    CanonicalName = "job2",
                    LastReviewed = DateTime.UtcNow.AddDays(-1),
                },
            };
        }

        private IList<SummaryApiModel> GetSummaryApiModels()
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