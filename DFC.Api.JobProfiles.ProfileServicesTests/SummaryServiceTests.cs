using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.ProfileServicesTests
{
    public class SummaryServiceTests
    {
        private const string RequestUrl = "http://Something.com";

        [Fact]
        public async Task GetSummaryListReturnsApiModelsWithCorrectUrlPrefixWhenDataExists()
        {
            var dataModels = GetSummaryDataModels();
            var repository = A.Fake<ICosmosRepository<SummaryDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SummaryDataModel, SummaryDataModel>>>.Ignored, null)).Returns(dataModels);

            var apiModels = GetSummaryApiModels();
            var mapper = A.Fake<IMapper>();
            A.CallTo(() => mapper.Map<IEnumerable<SummaryApiModel>>(A<IEnumerable<SummaryDataModel>>.Ignored)).Returns(apiModels);

            var summaryService = new SummaryService(repository, mapper);

            var result = await summaryService.GetSummaryList(RequestUrl).ConfigureAwait(false);
            var resultArray = result.ToArray();

            for (var i = 0; i < resultArray.Length; i++)
            {
                Assert.Equal($"{RequestUrl}/{dataModels[i].CanonicalName}", resultArray[i].Url);
            }
        }

        [Fact]
        public async Task GetSummaryListReturnsNullWhenDataDoesntExist()
        {
            var repository = A.Fake<ICosmosRepository<SummaryDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SummaryDataModel, SummaryDataModel>>>.Ignored, null)).Returns((IList<SummaryDataModel>)null);
            var mapper = A.Fake<IMapper>();
            var summaryService = new SummaryService(repository, mapper);

            var result = await summaryService.GetSummaryList(RequestUrl).ConfigureAwait(false);

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