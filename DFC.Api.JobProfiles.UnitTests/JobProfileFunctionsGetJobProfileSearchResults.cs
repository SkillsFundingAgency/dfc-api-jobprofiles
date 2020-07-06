using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public JobProfileFunctionsGetJobProfileSearchResults()
        {
            const string fakeHostName = "DummyHostName";

            httpRequest = A.Fake<HttpRequest>();
            httpRequest.HttpContext.Request.Scheme = "http";
            httpRequest.HttpContext.Request.Host = new HostString(fakeHostName);

            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var correlationProvider = new RequestHeaderCorrelationIdProvider(httpContextAccessor);
            using var telemetryConfig = new TelemetryConfiguration();
            var telemetryClient = new TelemetryClient(telemetryConfig);
            var logger = new LogService(correlationProvider, telemetryClient);
            var correlationResponse = new ResponseWithCorrelation(correlationProvider, httpContextAccessor);

            functionApp = new JobProfileFunctions(logger, correlationResponse);
            searchService = A.Fake<ISearchService>();
        }

        [Fact]
        public async Task GetJobProfileSearchResultsTestsReturnsOKAndViewModelWhenReturnedFromProfileDataService()
        {
            // Arrange
            const string searchTerm = "nurse";
            const int page = 2;
            const int pageSize = 3;
            var expectedResult = GetJobProfileSearchApiModel();
            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns(expectedResult);

            expectedResult.CurrentPage = page;
            expectedResult.PageSize = pageSize;

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchService, searchTerm).ConfigureAwait(false);

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
            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns((SearchApiModel)null);

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchService, searchTerm).ConfigureAwait(false);

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