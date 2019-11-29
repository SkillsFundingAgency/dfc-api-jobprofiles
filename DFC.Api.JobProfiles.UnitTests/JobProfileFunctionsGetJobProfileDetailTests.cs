using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var correlationProvider = new RequestHeaderCorrelationIdProvider(httpContextAccessor);
            var telemetryClient = new TelemetryClient();
            var logger = new LogService(correlationProvider, telemetryClient);
            var correlationResponse = new ResponseWithCorrelation(correlationProvider, httpContextAccessor);

            functionApp = new JobProfileFunctions(logger, correlationResponse);
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
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            okResult.Value.Should().BeEquivalentTo(expectedModel);
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
}