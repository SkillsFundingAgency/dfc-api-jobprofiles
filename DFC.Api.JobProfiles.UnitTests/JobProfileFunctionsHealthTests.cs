using AutoMapper;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Middleware;
using FakeItEasy;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.UnitTests
{
    public class JobProfileFunctionsHealthTests
    {
        private readonly HttpRequest httpRequest;
        private readonly JobProfileFunctions functionApp;

        public JobProfileFunctionsHealthTests()
        {
            httpRequest = A.Fake<HttpRequest>();
            var functionContextAccessor = A.Fake<IFunctionContextAccessor>();
            var fakeSharedContentRedis = A.Fake<ISharedContentRedisInterface>();
            var summaryService = A.Fake<ISummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            var fakeDetailService = A.Fake<IProfileDataService>();
            var fakeSearchService = A.Fake<ISearchService>();
            var mapper = A.Fake<IMapper>(); var correlationProvider = new RequestHeaderCorrelationIdProvider(functionContextAccessor);
            using var telemetryConfig = new TelemetryConfiguration();
            var telemetryClient = new TelemetryClient(telemetryConfig);
            var logger = new LogService(correlationProvider, telemetryClient);
            var correlationResponse = new ResponseWithCorrelation(correlationProvider, functionContextAccessor);

            functionApp = new JobProfileFunctions(logger, correlationResponse, fakeSharedContentRedis, mapper, functionContextAccessor, summaryService, healthCheckService, fakeSearchService, fakeDetailService);
        }

        [Fact]
        public void PingReturnsOKStatusResult()
        {
            // Act
            var result = functionApp.Ping(httpRequest);

            // Assert
            var statusResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, statusResult.StatusCode);
        }

        [Fact]
        public async Task HealthCheckReturnsOKStatusResultWhenChildAppIsHealthy()
        {
            // Arrange
            var dataService = A.Fake<IProfileDataService>();
            A.CallTo(() => dataService.PingAsync()).Returns(true);

            // Act
            var result = await functionApp.HealthCheck(httpRequest).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, statusResult.StatusCode);
        }

        [Fact]
        public async Task HealthCheckReturnsServiceUnavailableStatusResultWhenPingToCosmosReturnsNoResults()
        {
            // Arrange
            var dataService = A.Fake<IProfileDataService>();
            A.CallTo(() => dataService.PingAsync()).Returns(false);

            // Act
            var result = await functionApp.HealthCheck(httpRequest).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);
        }

        [Fact]
        public async Task HealthCheckReturnsServiceUnavailableStatusResultWhenPingToCosmosThrowsException()
        {
            // Arrange
            var dataService = A.Fake<IProfileDataService>();
            A.CallTo(() => dataService.PingAsync()).Throws<HttpRequestException>();

            // Act
            var result = await functionApp.HealthCheck(httpRequest).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);
        }
    }
}