/*using AutoMapper;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Middleware;
using FakeItEasy;
using FluentAssertions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.UnitTests
{
    public class JobProfileFunctionsSummaryListTests
    {
        private readonly HttpRequest httpRequest;
        private readonly ISummaryService fakeSummaryService;
        private readonly JobProfileFunctions functionApp;

        public JobProfileFunctionsSummaryListTests()
        {
            httpRequest = A.Fake<HttpRequest>();
            fakeSummaryService = A.Fake<ISummaryService>();
            var functionContextAccessor = A.Fake<IFunctionContextAccessor>();
            var fakeSharedContentRedis = A.Fake<ISharedContentRedisInterface>();
            var summaryService = A.Fake<ISummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            var fakeSearchService = A.Fake<ISearchService>();
            var fakeDetailService = A.Fake<IProfileDataService>();
            var mapper = A.Fake<IMapper>();
            var correlationProvider = new RequestHeaderCorrelationIdProvider(functionContextAccessor);
            using var telemetryConfig = new TelemetryConfiguration();
            var telemetryClient = new TelemetryClient(telemetryConfig);
            var logger = new LogService(correlationProvider, telemetryClient);
            var correlationResponse = new ResponseWithCorrelation(correlationProvider, functionContextAccessor);

            functionApp = new JobProfileFunctions(logger, correlationResponse, fakeSharedContentRedis, mapper, functionContextAccessor, summaryService, healthCheckService, fakeSearchService, fakeDetailService);
        }

        [Fact]
        public async Task GetSummaryListReturnsOKAndViewModelsWhenReturnedFromSummaryService()
        {
            // Arrange
            var expectedModels = GetSummaryApiModels();
            A.CallTo(() => fakeSummaryService.GetSummaryList(A<string>.Ignored)).Returns(expectedModels);

            // Act
            var result = await functionApp.GetSummaryList(httpRequest).ConfigureAwait(false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<IList<SummaryApiModel>>(okResult.Value.ToString());
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            deserialisedResult.Should().BeEquivalentTo(expectedModels);
        }

        [Fact]
        public async Task GetSummaryListReturnsNoContentResultWhenNullReturnedFromSummaryService()
        {
            // Arrange
            A.CallTo(() => fakeSummaryService.GetSummaryList(A<string>.Ignored)).Returns((IList<SummaryApiModel>)null);

            // Act
            var result = await functionApp.GetSummaryList(httpRequest).ConfigureAwait(false);

            // Assert
            var noContentResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        private IList<SummaryApiModel> GetSummaryApiModels()
        {
            return new List<SummaryApiModel>
            {
                new SummaryApiModel
                {
                    Title = "JobTitle1",
                    Url = "/job1",
                    LastUpdated = DateTime.UtcNow,
                },
                new SummaryApiModel
                {
                    Title = "JobTitle2",
                    Url = "/job2",
                    LastUpdated = DateTime.UtcNow.AddDays(-1),
                },
            };
        }
    }
}*/