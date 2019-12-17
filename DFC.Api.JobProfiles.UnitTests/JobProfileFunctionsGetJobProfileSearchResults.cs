using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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
            var telemetryClient = new TelemetryClient();
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
            const int page = 1;
            const int pageSize = 10;
            var expectedResult = GetJobProfileSearchApiModel();
            A.CallTo(() => searchService.GetResutsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns(expectedResult);

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchService, searchTerm, page, pageSize).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchService.GetResutsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<IList<SearchApiModel>>(okResult.Value.ToString());
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            deserialisedResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetJobProfileSearchResultsTestsReturnsNoContentWhenNullReturnedFromProfileDataService()
        {
            // Arrange
            const string searchTerm = "nurse";
            const int page = 1;
            const int pageSize = 10;
            A.CallTo(() => searchService.GetResutsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns((IList<SearchApiModel>)null);

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchService, searchTerm, page, pageSize).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchService.GetResutsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).MustHaveHappenedOnceExactly();

            var noContentResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        private IList<SearchApiModel> GetJobProfileSearchApiModel()
        {
            return new List<SearchApiModel>
            {
                new SearchApiModel
                {
                    Rank = 1,
                    ResultItemTitle = "Title 1",
                    ResultItemAlternativeTitle = "Alt title 1",
                    ResultItemOverview = "Overview 1",
                    ResultItemSalaryRange = "Salary range 1",
                    ResultItemUrlName = "url name 1",
                    JobProfileCategoriesWithUrl = new[] { "category 1.1", "Categopry 1.2" },
                    Score = 100,
                },
                new SearchApiModel
                {
                    Rank = 2,
                    ResultItemTitle = "Title 2",
                    ResultItemAlternativeTitle = "Alt title 2",
                    ResultItemOverview = "Overview 2",
                    ResultItemSalaryRange = "Salary range 2",
                    ResultItemUrlName = "url name 2",
                    JobProfileCategoriesWithUrl = new[] { "category 2.1", "Categopry 2.2" },
                    Score = 200,
                },
                new SearchApiModel
                {
                    Rank = 3,
                    ResultItemTitle = "Title 3",
                    ResultItemAlternativeTitle = "Alt title 3",
                    ResultItemOverview = "Overview 3",
                    ResultItemSalaryRange = "Salary range 3",
                    ResultItemUrlName = "url name 3",
                    JobProfileCategoriesWithUrl = new[] { "category 3.1", "Categopry 3.2" },
                    Score = 300,
                },
                new SearchApiModel
                {
                    Rank = 4,
                    ResultItemTitle = "Title 4",
                    ResultItemAlternativeTitle = "Alt title 4",
                    ResultItemOverview = "Overview 4",
                    ResultItemSalaryRange = "Salary range 4",
                    ResultItemUrlName = "url name 4",
                    JobProfileCategoriesWithUrl = new[] { "category 4.1", "Categopry 4.2" },
                    Score = 400,
                },
            };
        }
    }
}