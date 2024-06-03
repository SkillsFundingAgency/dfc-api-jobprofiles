using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using FakeItEasy;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.UnitTests
{
    public class JobProfileFunctionsHealthTests
    {
        private readonly HttpRequest httpRequest;
        private readonly JobProfileFunctions functionApp;
        private readonly IResponseWithCorrelation fakeCorrelation;
        private readonly IProfileDataService dataService;
        private readonly HealthCheckService healthCheckService;


        public JobProfileFunctionsHealthTests()
        {
            httpRequest = A.Fake<HttpRequest>();
            var summaryService = A.Fake<ISummaryService>();
            healthCheckService = A.Fake<HealthCheckService>();
            dataService = A.Fake<IProfileDataService>();
            using var telemetryConfig = new TelemetryConfiguration();
            ILogService logService = A.Fake<LogService>();
            var searchService = A.Fake<ISearchService>();
            fakeCorrelation = A.Fake<IResponseWithCorrelation>();

            functionApp = new JobProfileFunctions(logService, fakeCorrelation, summaryService, healthCheckService, searchService, dataService);
        }

        [Fact]
        public void PingReturnsOKStatusResult()
        {
            // Act
            A.CallTo(() => fakeCorrelation.ResponseObjectWithCorrelationId(A<object>.Ignored))
               .Returns(new StatusCodeResult(200));
            var result = functionApp.Ping(httpRequest);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, statusResult.StatusCode);
        }

        [Fact]
        public async Task HealthControllerHealthReturnsSuccessWhenHealthy()
        {
            // Arrange
            var service = CreateHealthChecksService(b =>
            {
                b.AddAsyncCheck("HealthyCheck", _ => Task.FromResult(HealthCheckResult.Healthy()));
            });
            A.CallTo(() => fakeCorrelation.ResponseObjectWithCorrelationId(A<object>.Ignored))
              .Returns(new StatusCodeResult(200));

            // Act
            var healthCheckResult = await service.CheckHealthAsync();

            // Assert
            Assert.Collection(
                healthCheckResult.Entries,
                actual =>
                {
                    Assert.Equal("HealthyCheck", actual.Key);
                    Assert.Equal(HealthStatus.Healthy, actual.Value.Status);
                    Assert.Null(actual.Value.Exception);
                });

        }

        [Fact]
        public async Task HealthControllerHealthReturnsServiceUnavailableWhenUnhealthy()
        {
            // Arrange
            var service = CreateHealthChecksService(b =>
            {
                b.AddAsyncCheck("timeout", async (ct) =>
                {
                    await Task.Delay(2000, ct);
                    return HealthCheckResult.Unhealthy();
                }, timeout: TimeSpan.FromMilliseconds(100));
            });

            var result = await functionApp.HealthCheck(httpRequest).ConfigureAwait(false);

            // Act
            var healthCheckResult = await service.CheckHealthAsync();

            // Assert
            Assert.Collection(
                healthCheckResult.Entries,
                actual =>
                {
                    Assert.Equal("timeout", actual.Key);
                    Assert.Equal(HealthStatus.Unhealthy, actual.Value.Status);
                    var statusResult = Assert.IsType<StatusCodeResult>(result);
                    A.Equals((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);
                });
        }

        protected HealthCheckService CreateHealthChecksService(Action<IHealthChecksBuilder> configure)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddOptions();

            var builder = services.AddHealthChecks();
            configure?.Invoke(builder);

            return services.BuildServiceProvider(validateScopes: true).GetRequiredService<HealthCheckService>();
        }
    }
}