using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DFC.Api.JobProfiles.ProfileServices.UnitTests.DetailSegmentTests
{
    public class SkillsTests
    {
        [Fact]
        public async Task GetSkillsDataSuccessAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = GetMapperInstance();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();
            var expectedSkillsResult = GetSkillsData();

            var canonicalName = "biochemist";
            var filter = "PUBLISHED";


            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileSkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(expectedResult);
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<SkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(expectedSkillsResult);

            //Act
            var response = await profileDataService.GetSkillSegmentAsync(canonicalName, filter);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileSkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<SkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(response);
            response.Should().BeOfType(typeof(WhatItTakesApiModel));
        }

        [Fact]
        public async Task GetSkillsDataNoSuccessAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(logger, mapper, sharedContentRedisInterface);

            var canonicalName = "biochemist";
            var filter = "PUBLISHED";

            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileSkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(new JobProfileSkillsResponse());
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<SkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(new SkillsResponse());

            //Act
            var response = await profileDataService.GetSkillSegmentAsync(canonicalName, filter);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileSkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<SkillsResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            response.Should().BeOfType(typeof(WhatItTakesApiModel));
        }

        private static JobProfileSkillsResponse GetExpectedData()
        {
            var expectedResult = new JobProfileSkillsResponse();
            var list = new List<JobProfileSkill>()
            {
                new JobProfileSkill
                {
                    DigitalSkills = new DigitalSkills
                    {
                        ContentItems = new List<DigitalSkillsContentItem>
                        {
                            new DigitalSkillsContentItem()
                            {
                                Description = "Skill1Desc",
                                DisplayText = "Skill1",
                                GraphSync = new ()
                                {
                                    NodeId = "29d1a617-92b7-446f-81a1-070e69b00aa9",
                                },
                            },
                        },
                    },
                    DisplayText = "BioSkill",
                    Otherrequirements = new ()
                   {
                       Html = "ExampleHTML",
                   },
                    PageLocation = new ()
                   {
                       DefaultPageForLocation = false,
                       FullUrl = "/biochemist",
                       UrlName = "biochemist",
                   },
                    Relatedrestrictions = new Relatedrestrictions
                   {
                       ContentItems = new List<RelatedrestrictionsContentItem>
                       {
                           new RelatedrestrictionsContentItem()
                           {
                               DisplayText = "Restriction1",
                               GraphSync = new ()
                               {
                                   NodeId = "29d1a617-92b7-446f-81a1-070e69b00aa9",
                               },
                               Info = new ()
                               {
                                   Html = "InfoExampleText",
                               },
                           },
                       },
                   },
                    Relatedskills = new Relatedskills
                   {
                       ContentItems = new List<RelatedSkill>
                       {
                           new RelatedSkill()
                           {
                               DisplayText = "RelatedSkill1",
                               RelatedSkillDesc = "RelatedSkillDesc",
                               GraphSync = new ()
                               {
                                   NodeId = "29d1a617-92b7-446f-81a1-070e69b00aa9",
                               },
                               ONetAttributeType = "testONet",
                               ONetRank = "3.185",
                               Ordinal = 1,
                               RelatedSOCcode = "TestSocCode",
                           },
                       },
                   },
                },
            };
            expectedResult.JobProfileSkills = list;
            return expectedResult;
        }

        private static SkillsResponse GetSkillsData()
        {
            var expectedResult = new SkillsResponse();

            var list = new List<DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles.Skills>
            {
                new DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles.Skills
                {
                    Description = "Skill1Desc",
                    DisplayText = "RelatedSkillDesc",
                    GraphSync = new()
                    {
                        NodeId = "29d1a617-92b7-446f-81a1-070e69b00aa9",
                    },
                    ONetElementId = "12345",
                },
            };
            expectedResult.Skill = list;
            return expectedResult;
        }

        private IMapper GetMapperInstance()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApiAutoMapperProfile());
            });
            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}
