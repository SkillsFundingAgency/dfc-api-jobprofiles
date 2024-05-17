/*using AutoMapper;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
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
    public class JobProfileFunctionsGetJobProfileDetailTests
    {
        private const string CanonicalName = "jobCanonicalName1";
        private readonly HttpRequest httpRequest;
        private readonly IProfileDataService profileDataService;
        private readonly JobProfileFunctions functionApp;

        public JobProfileFunctionsGetJobProfileDetailTests()
        {
            const string fakeHostName = "DummyHostName";

            httpRequest = A.Fake<HttpRequest>();
            httpRequest.HttpContext.Request.Scheme = "http";
            httpRequest.HttpContext.Request.Host = new HostString(fakeHostName);

            profileDataService = A.Fake<IProfileDataService>();
            var functionContextAccessor = A.Fake<IFunctionContextAccessor>();
            var fakeSharedContentRedis = A.Fake<ISharedContentRedisInterface>();
            var summaryService = A.Fake<ISummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            var fakeSearchService = A.Fake<ISearchService>();
            var mapper = A.Fake<IMapper>();
            var correlationProvider = new RequestHeaderCorrelationIdProvider(functionContextAccessor);
            using var telemetryConfig = new TelemetryConfiguration();
            var telemetryClient = new TelemetryClient(telemetryConfig);
            var logger = new LogService(correlationProvider, telemetryClient);
            var correlationResponse = new ResponseWithCorrelation(correlationProvider, functionContextAccessor);

            functionApp = new JobProfileFunctions(logger, correlationResponse, fakeSharedContentRedis, mapper, functionContextAccessor, summaryService, healthCheckService, fakeSearchService);
        }

        [Fact]
        public async Task GetJobProfileDetailTestsReturnsOKAndViewModelWhenReturnedFromProfileDataService()
        {
            // Arrange
            var expectedModel = GetJobProfileApiModel();
            A.CallTo(() => profileDataService.GetJobProfile(A<string>.Ignored)).Returns(expectedModel);

            // Act
            var result = await functionApp.GetJobProfileDetail(httpRequest, CanonicalName, profileDataService).ConfigureAwait(false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<JobProfileApiModel>(okResult.Value.ToString());
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            deserialisedResult.Should().BeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task GetJobProfileDetailTestsReturnsOKWhenRelatedCareersDoesNotExist()
        {
            // Arrange
            var expectedModel = GetJobProfileApiModel();
            expectedModel.RelatedCareers = null;
            A.CallTo(() => profileDataService.GetJobProfile(A<string>.Ignored)).Returns(expectedModel);

            // Act
            var result = await functionApp.GetJobProfileDetail(httpRequest, CanonicalName, profileDataService).ConfigureAwait(false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<JobProfileApiModel>(okResult.Value.ToString());
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            deserialisedResult.Should().BeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task GetJobProfileDetailTestsReturnsNoContentWhenNullReturnedFromProfileDataService()
        {
            // Arrange
            A.CallTo(() => profileDataService.GetJobProfile(A<string>.Ignored)).Returns((JobProfileApiModel)null);

            // Act
            var result = await functionApp.GetJobProfileDetail(httpRequest, CanonicalName, profileDataService).ConfigureAwait(false);

            // Assert
            var noContentResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        private JobProfileApiModel GetJobProfileApiModel()
        {
            return new JobProfileApiModel
            {
                Title = "JobTitle1",
                Url = "/job1",
                LastUpdatedDate = DateTime.UtcNow,
                Overview = "Some Overview text",
                WhatItTakes = new WhatItTakesApiModel
                {
                    DigitalSkillsLevel = "Digital Skill level",
                    RestrictionsAndRequirements = new RestrictionsAndRequirementsApiModel
                    {
                        OtherRequirements = new List<string> { "Other Requirement 1", "Other Requirement 2" },
                        RelatedRestrictions = new List<string> { "Related Restriction 1", "Related Restriction 2" },
                    },
                    Skills = new List<RelatedSkillsApiModel>
                    {
                        new RelatedSkillsApiModel
                        {
                            Description = "Skill Desc 1",
                            ONetAttributeType = "ONetAttributeType 1",
                            ONetElementId = "ONetElementId 1",
                            ONetRank = "ONetRank 1",
                        },
                    },
                },
                RelatedCareers = new List<RelatedCareerApiModel>
                {
                    new RelatedCareerApiModel
                    {
                        Title = "Related career 1",
                        Url = "/relatedCareer1",
                    },
                    new RelatedCareerApiModel
                    {
                        Title = "Related career 2",
                        Url = "/relatedCareer2",
                    },
                },
            };
        }
    }
}*/