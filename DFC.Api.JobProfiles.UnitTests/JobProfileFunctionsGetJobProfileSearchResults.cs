using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Api.JobProfiles.Data.ContractResolver;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.UnitTests
{
    public class JobProfileFunctionsGetJobProfileSearchResults
    {
        private readonly HttpRequest httpRequest;
        private readonly ISearchService searchService;
        private readonly JobProfileFunctions functionApp;
        private readonly IResponseWithCorrelation fakeCorrelation;

        public JobProfileFunctionsGetJobProfileSearchResults()
        {
            const string fakeHostName = "DummyHostName";

            httpRequest = A.Fake<HttpRequest>();
            httpRequest.HttpContext.Request.Scheme = "http";
            httpRequest.HttpContext.Request.Host = new HostString(fakeHostName);

            var summaryService = A.Fake<ISummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            var fakeDetailService = A.Fake<IProfileDataService>();
            using var telemetryConfig = new TelemetryConfiguration();
            ILogService logService = A.Fake<LogService>();
            searchService = A.Fake<ISearchService>();
            fakeCorrelation = A.Fake<IResponseWithCorrelation>();

            functionApp = new JobProfileFunctions(logService, fakeCorrelation, summaryService, healthCheckService, searchService, fakeDetailService);
        }

        [Fact]
        public async Task GetJobProfileSearchResultsTestsReturnsOkAndViewModelWhenReturnedFromProfileDataService()
        {
            // Arrange
            const string searchTerm = "nurse";
            const int page = 2;
            const int pageSize = 3;
            var expectedResult = GetJobProfileSearchApiModel();
            expectedResult.CurrentPage = page;
            expectedResult.PageSize = pageSize;
            var settings = new JsonSerializerSettings { ContractResolver = new OrderedContractResolver() };
            var orderedModel = JsonConvert.SerializeObject(expectedResult, Formatting.Indented, settings);

            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns(expectedResult);
            A.CallTo(() => fakeCorrelation.ResponseObjectWithCorrelationId(A<object>.Ignored))
                .Returns(new OkObjectResult(JsonConvert.DeserializeObject(orderedModel)));

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchTerm).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<SearchApiModel>(okResult.Value.ToString());
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            deserialisedResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetJobProfileSearchResultsTestsReturnsNoContentWhenNullReturnedFromProfileDataService()
        {
            // Arrange
            const string searchTerm = "nurse";

            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns(new SearchApiModel());
            A.CallTo(() => fakeCorrelation.ResponseWithCorrelationId(A<HttpStatusCode>.Ignored))
                .Returns(new StatusCodeResult(204));

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchTerm).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).MustHaveHappenedOnceExactly();

            var noContentResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        private SearchApiModel GetJobProfileSearchApiModel()
        {
            var result = new SearchApiModel
            {
                Results = new List<SearchItemApiModel>
                {
                    new SearchItemApiModel
                    {
                        ResultItemTitle = "Title 1",
                        ResultItemAlternativeTitle = "Alt title 1",
                        ResultItemOverview = "Overview 1",
                        ResultItemSalaryRange = "Salary range 1",
                        ResultItemUrlName = "url name 1",
                        JobProfileCategories = new List<JobProfileCategoryApiModel>
                        {
                            new JobProfileCategoryApiModel
                            {
                                Name = "category-1",
                                Title = "Category 1",
                            },
                        },
                    },
                    new SearchItemApiModel
                    {
                        ResultItemTitle = "Title 2",
                        ResultItemAlternativeTitle = "Alt title 2",
                        ResultItemOverview = "Overview 2",
                        ResultItemSalaryRange = "Salary range 2",
                        ResultItemUrlName = "url name 2",
                        JobProfileCategories = new List<JobProfileCategoryApiModel>
                        {
                            new JobProfileCategoryApiModel
                            {
                                Name = "category-2",
                                Title = "Category 2",
                            },
                        },
                    },
                    new SearchItemApiModel
                    {
                        ResultItemTitle = "Title 3",
                        ResultItemAlternativeTitle = "Alt title 3",
                        ResultItemOverview = "Overview 3",
                        ResultItemSalaryRange = "Salary range 3",
                        ResultItemUrlName = "url name 3",
                        JobProfileCategories = new List<JobProfileCategoryApiModel>
                        {
                            new JobProfileCategoryApiModel
                            {
                                Name = "category-3",
                                Title = "Category 3",
                            },
                        },
                    },
                    new SearchItemApiModel
                    {
                        ResultItemTitle = "Title 4",
                        ResultItemAlternativeTitle = "Alt title 4",
                        ResultItemOverview = "Overview 4",
                        ResultItemSalaryRange = "Salary range 4",
                        ResultItemUrlName = "url name 4",
                        JobProfileCategories = new List<JobProfileCategoryApiModel>
                        {
                            new JobProfileCategoryApiModel
                            {
                                Name = "category-4",
                                Title = "Category 4",
                            },
                        },
                    },
                },
            };

            result.Count = result.Results.Count();

            return result;
        }
    }
}