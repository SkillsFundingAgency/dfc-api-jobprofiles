using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.ContractResolver;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using FakeItEasy;
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
        private readonly IProfileDataService dataService;
        private readonly JobProfileFunctions functionApp;
        private readonly IResponseWithCorrelation fakeCorrelation;

        public JobProfileFunctionsGetJobProfileDetailTests()
        {
            const string fakeHostName = "DummyHostName";

            httpRequest = A.Fake<HttpRequest>();
            httpRequest.HttpContext.Request.Scheme = "http";
            httpRequest.HttpContext.Request.Host = new HostString(fakeHostName);
            httpRequest.Headers.Add("X-Forwarded-APIM-Url", "/job1");

            var summaryService = A.Fake<ISummaryService>();
            var healthCheckService = A.Fake<HealthCheckService>();
            dataService = A.Fake<IProfileDataService>();
            using var telemetryConfig = new TelemetryConfiguration();
            ILogService logService = A.Fake<LogService>();
            var searchService = A.Fake<ISearchService>();
            fakeCorrelation = A.Fake<IResponseWithCorrelation>();

            functionApp = new JobProfileFunctions(logService, fakeCorrelation, summaryService, healthCheckService, searchService, dataService);
        }

        [Fact]
        public async Task GetJobProfileDetailTestsReturnsOkAndViewModelWhenReturnedFromProfileDataService()
        {
            // Arrange
            var expectedModel = GetJobProfileApiModel();
            var settings = new JsonSerializerSettings { ContractResolver = new OrderedContractResolver() };
            var orderedModel = JsonConvert.SerializeObject(expectedModel, Formatting.Indented, settings);

            A.CallTo(() => dataService.GetJobProfile(A<string>.Ignored)).Returns(expectedModel);
            A.CallTo(() => fakeCorrelation.ResponseObjectWithCorrelationId(A<object>.Ignored))
                .Returns(new OkObjectResult(JsonConvert.DeserializeObject(orderedModel)));            


            // Act
            var result = await functionApp.GetJobProfileDetail(httpRequest, CanonicalName).ConfigureAwait(false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<JobProfileApiModel>(okResult.Value.ToString());
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetJobProfileDetailTestsReturnsOkWhenRelatedCareersDoesNotExist()
        {
            // Arrange
            var expectedModel = GetJobProfileApiModel();
            expectedModel.RelatedCareers = null;
            var settings = new JsonSerializerSettings { ContractResolver = new OrderedContractResolver() };
            var orderedModel = JsonConvert.SerializeObject(expectedModel, Formatting.Indented, settings);

            A.CallTo(() => dataService.GetJobProfile(A<string>.Ignored)).Returns(expectedModel);
            A.CallTo(() => fakeCorrelation.ResponseObjectWithCorrelationId(A<object>.Ignored))
                .Returns(new OkObjectResult(JsonConvert.DeserializeObject(orderedModel)));

            // Act
            var result = await functionApp.GetJobProfileDetail(httpRequest, CanonicalName).ConfigureAwait(false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deserialisedResult = JsonConvert.DeserializeObject<JobProfileApiModel>(okResult.Value.ToString());
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetJobProfileDetailTestsReturnsNoContentWhenNullReturnedFromProfileDataService()
        {
            // Arrange
            A.CallTo(() => dataService.GetJobProfile(A<string>.Ignored)).Returns((JobProfileApiModel)null);
            A.CallTo(() => fakeCorrelation.ResponseWithCorrelationId(A<HttpStatusCode>.Ignored))
                .Returns(new StatusCodeResult(204));

            // Act
            var result = await functionApp.GetJobProfileDetail(httpRequest, CanonicalName).ConfigureAwait(false);

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
}