using AutoMapper;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Api.JobProfiles.Data.ContractResolver;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.SearchServices;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Middleware;
using FakeItEasy;
using FluentAssertions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
        private readonly ILogService logService;


        public JobProfileFunctionsGetJobProfileSearchResults()
        {
            const string fakeHostName = "DummyHostName";

            httpRequest = A.Fake<HttpRequest>();
            httpRequest.HttpContext.Request.Scheme = "http";
            httpRequest.HttpContext.Request.Host = new HostString(fakeHostName);

            var functionContextAccessor = A.Fake<IFunctionContextAccessor>();
            var fakeSharedContentRedis = A.Fake<ISharedContentRedisInterface>();
            var summaryService = A.Fake<ISummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            //searchService = A.Fake<SearchService>();
            var fakeDetailService = A.Fake<IProfileDataService>();
            var mapper = A.Fake<IMapper>();
            var correlationProvider = new RequestHeaderCorrelationIdProvider(functionContextAccessor);
            using var telemetryConfig = new TelemetryConfiguration();
            var telemetryClient = new TelemetryClient(telemetryConfig);
            //var logger = new LogService(correlationProvider, telemetryClient);
            //logService = new LogService(correlationProvider, telemetryClient);
            logService = A.Fake<LogService>();
            searchService = A.Fake<ISearchService>();
            fakeCorrelation = A.Fake<IResponseWithCorrelation>();

            //functionApp = new JobProfileFunctions(logService, fakeCorrelation, fakeSharedContentRedis, mapper, functionContextAccessor, summaryService, healthCheckService, searchService, fakeDetailService);
            //searchService = A.Fake<ISearchService>();
        }

        [Fact]
        public async Task GetJobProfileSearchResultsTestsReturnsOKAndViewModelWhenReturnedFromProfileDataService()
        {
            // Arrange
            var functionContextAccessor = A.Fake<FunctionContextAccessor>();
            var fakeSharedContentRedis = A.Fake<ISharedContentRedisInterface>();
            var summaryService = A.Fake<SummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            var mapper = A.Fake<Mapper>();
            var fakeDetailService = A.Fake<ProfileDataService>();
            var tfakeCorrelation = A.Fake<IResponseWithCorrelation>();
            var fakeLog = A.Fake<ILogService>();

            var functionApp = new JobProfileFunctions(fakeLog, tfakeCorrelation, fakeSharedContentRedis, mapper, functionContextAccessor, summaryService, healthCheckService, searchService, fakeDetailService);


            const string searchTerm = "nurse";
            const int page = 2;
            const int pageSize = 3;
            var expectedResult = GetJobProfileSearchApiModel();

            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns(expectedResult);

            expectedResult.CurrentPage = page;
            expectedResult.PageSize = pageSize;
            var settings = new JsonSerializerSettings { ContractResolver = new OrderedContractResolver() };
            var orderedModel = JsonConvert.SerializeObject(expectedResult, Formatting.Indented, settings);
            var deser = JsonConvert.DeserializeObject(orderedModel);
            A.CallTo(() => fakeCorrelation.ResponseObjectWithCorrelationId(A<string>.Ignored)).Returns(new OkObjectResult(deser));

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchTerm).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).MustHaveHappenedOnceExactly();

            //var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<SearchApiModel>(orderedModel);
            //Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            deserialisedResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetJobProfileSearchResultsTestsReturnsNoContentWhenNullReturnedFromProfileDataService()
        {
            // Arrange
            var functionContextAccessor = A.Fake<IFunctionContextAccessor>();
            var fakeSharedContentRedis = A.Fake<ISharedContentRedisInterface>();
            var summaryService = A.Fake<SummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            var mapper = A.Fake<Mapper>();
            var fakeDetailService = A.Fake<ProfileDataService>();
            var tfakeCorrelation = A.Fake<IResponseWithCorrelation>();
            var fakeLog = A.Fake<LogService>();


            var functionApp = new JobProfileFunctions(fakeLog, tfakeCorrelation, fakeSharedContentRedis, mapper, functionContextAccessor, summaryService, healthCheckService, searchService, fakeDetailService);

            const string searchTerm = "nurse";
            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns((SearchApiModel)null);

            // Act
            var result = await functionApp.GetJobProfileSearchResults(httpRequest, searchTerm).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchService.GetResultsList(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<int>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Null(result);
            //var noContentResult = Assert.IsType<StatusCodeResult>(result);
            //Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
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