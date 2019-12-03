using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.Overview;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Data.Enums;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.ProfileServices.UnitTests
{
    public class ProfileDataServiceTests
    {
        private const string JobProfileName = "jobName1";
        private readonly ILogger defaultLogger;

        public ProfileDataServiceTests()
        {
            defaultLogger = A.Fake<ILogger>();
        }

        [Fact]
        public async Task GetJobProfileReturnsNullWhenDataDoesNotExists()
        {
            // Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SegmentDataModel, SegmentDataModel>>>.Ignored, A<Expression<Func<SegmentDataModel, bool>>>.Ignored)).Returns((IList<SegmentDataModel>)null);
            var dataService = new ProfileDataService(repository, defaultLogger);

            // Act
            var result = await dataService.GetJobProfile(JobProfileName).ConfigureAwait(false);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetJobProfileReturnsOverviewOnlyWhenOtherSegmentsDataDoesNotExists()
        {
            // Arrange
            var onlyOverviewSegmentDataModel = GetOnlyOverviewSegmentDataModel();
            var expectedOverview = GetOverviewApiModel();
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SegmentDataModel, SegmentDataModel>>>.Ignored, A<Expression<Func<SegmentDataModel, bool>>>.Ignored)).Returns(onlyOverviewSegmentDataModel);
            var dataService = new ProfileDataService(repository, defaultLogger);

            // Act
            var result = await dataService.GetJobProfile(JobProfileName).ConfigureAwait(false);

            // Assert
            Assert.True(result.Title.Equals(expectedOverview.Title, StringComparison.OrdinalIgnoreCase)
                        && result.Overview.Equals(expectedOverview.Overview, StringComparison.OrdinalIgnoreCase)
                        && result.Soc.Equals(expectedOverview.Soc, StringComparison.OrdinalIgnoreCase)
                        && result.Url.Equals(expectedOverview.Url, StringComparison.OrdinalIgnoreCase));

            Assert.True(result.HowToBecome is null &&
                result.CareerPathAndProgression is null &&
                result.RelatedCareers is null &&
                result.WhatItTakes is null &&
                result.WhatYouWillDo is null);
        }

        [Fact]
        public async Task GetJobProfileReturnsApiModelsWithCorrectUrlPrefixWhenDataExists()
        {
            // Arrange
            var dataModels = GetSegmentDataModel();
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SegmentDataModel, SegmentDataModel>>>.Ignored, A<Expression<Func<SegmentDataModel, bool>>>.Ignored)).Returns(dataModels);

            var dataService = new ProfileDataService(repository, defaultLogger);
            var expectedOverview = GetOverviewApiModel();

            // Act
            var result = await dataService.GetJobProfile(JobProfileName).ConfigureAwait(false);

            // Assert
            Assert.True(result.Title.Equals(expectedOverview.Title, StringComparison.OrdinalIgnoreCase)
                        && result.Overview.Equals(expectedOverview.Overview, StringComparison.OrdinalIgnoreCase)
                        && result.Soc.Equals(expectedOverview.Soc, StringComparison.OrdinalIgnoreCase)
                        && result.Url.Equals(expectedOverview.Url, StringComparison.OrdinalIgnoreCase));

            result.WhatItTakes.Should().BeEquivalentTo(GetWhatItakesApiModel());
            result.WhatYouWillDo.Should().BeEquivalentTo(GetWhatYouWillDoApiModel());
            result.RelatedCareers.Should().BeEquivalentTo(GetRelatedCareerApiModel());
            result.HowToBecome.Should().BeEquivalentTo(GetHowToBecomeApiModel());
            result.CareerPathAndProgression.Should().BeEquivalentTo(GetCareerPathAndProgressionApiModel());
        }

        [Fact]
        public async Task GetJobProfileReturnsDataWhenNOSegmentsDataExists()
        {
            // Arrange
            var noSegmentsDataModel = GetWithMissingSegmentsDataModel();
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            A.CallTo(() => repository.GetData(A<Expression<Func<SegmentDataModel, SegmentDataModel>>>.Ignored, A<Expression<Func<SegmentDataModel, bool>>>.Ignored)).Returns(noSegmentsDataModel);
            var dataService = new ProfileDataService(repository, defaultLogger);

            // Act
            var result = await dataService.GetJobProfile(JobProfileName).ConfigureAwait(false);

            // Assert
            Assert.True(result.Title is null
                        && result.Overview is null
                        && result.Soc is null
                        && result.Url is null);

            Assert.True(result.HowToBecome is null &&
                result.CareerPathAndProgression is null &&
                result.RelatedCareers is null &&
                result.WhatItTakes is null &&
                result.WhatYouWillDo is null);
        }

        private IList<SegmentDataModel> GetSegmentDataModel()
        {
            return new List<SegmentDataModel>
            {
                new SegmentDataModel
                {
                    CanonicalName = "job1",
                    Segments = new List<SegmentDetailModel>
                    {
                        new SegmentDetailModel
                        {
                            Segment = JobProfileSegment.Overview,
                            Json = JsonConvert.SerializeObject(GetOverviewApiModel()),
                        },
                        new SegmentDetailModel
                        {
                            Segment = JobProfileSegment.CareerPathsAndProgression,
                            Json = JsonConvert.SerializeObject(GetCareerPathAndProgressionApiModel()),
                        },
                        new SegmentDetailModel
                        {
                            Segment = JobProfileSegment.RelatedCareers,
                            Json = JsonConvert.SerializeObject(GetRelatedCareerApiModel()),
                        },
                        new SegmentDetailModel
                        {
                            Segment = JobProfileSegment.HowToBecome,
                            Json = JsonConvert.SerializeObject(GetHowToBecomeApiModel()),
                        },
                        new SegmentDetailModel
                        {
                            Segment = JobProfileSegment.WhatItTakes,
                            Json = JsonConvert.SerializeObject(GetWhatItakesApiModel()),
                        },
                        new SegmentDetailModel
                        {
                            Segment = JobProfileSegment.WhatYouWillDo,
                            Json = JsonConvert.SerializeObject(GetWhatYouWillDoApiModel()),
                        },
                    },
                },
            };
        }

        private IList<SegmentDataModel> GetOnlyOverviewSegmentDataModel()
        {
            return new List<SegmentDataModel>
            {
                new SegmentDataModel
                {
                    CanonicalName = "job1",
                    Segments = new List<SegmentDetailModel>
                    {
                        new SegmentDetailModel
                        {
                            Segment = JobProfileSegment.Overview,
                            Json = JsonConvert.SerializeObject(GetOverviewApiModel()),
                        },
                    },
                },
            };
        }

        private IList<SegmentDataModel> GetWithMissingSegmentsDataModel()
        {
            return new List<SegmentDataModel>
            {
                new SegmentDataModel
                {
                    CanonicalName = "job1",
                },
            };
        }

        private OverviewApiModel GetOverviewApiModel()
        {
            return new OverviewApiModel
            {
                Title = "job1",
                Overview = "Overview text",
                Soc = "12345",
                Url = "job1",
            };
        }

        private WhatItTakesApiModel GetWhatItakesApiModel()
        {
            return new WhatItTakesApiModel
            {
                DigitalSkillsLevel = "Digital Skill 1",
                RestrictionsAndRequirements = new RestrictionsAndRequirementsApiModel
                {
                    RelatedRestrictions = new List<string> { "Related restriction 1" },
                    OtherRequirements = new List<string> { "Other requirements 1" },
                },
                Skills = new List<RelatedSkillsApiModel>
                {
                    new RelatedSkillsApiModel
                    {
                        Description = "Skill desc 1",
                        ONetAttributeType = "ONetAttributeType1",
                        ONetElementId = "ONetElementId1",
                        ONetRank = "ONetRank1",
                    },
                },
            };
        }

        private WhatYouWillDoApiModel GetWhatYouWillDoApiModel()
        {
            return new WhatYouWillDoApiModel
            {
                WYDDayToDayTasks = new List<string> { "Task1" },
                WorkingEnvironment = new WorkingEnvironmentApiModel
                {
                    Environment = "Environment1",
                    Location = "Location 1",
                    Uniform = "Uniform 1",
                },
            };
        }

        private List<RelatedCareerApiModel> GetRelatedCareerApiModel()
        {
            return new List<RelatedCareerApiModel>
            {
               new RelatedCareerApiModel
               {
                   Title = "Related Career 1",
                   Url = "relatedcareer1",
               },
               new RelatedCareerApiModel
               {
                   Title = "Related Career 2",
                   Url = "relatedcareer2",
               },
            };
        }

        private HowToBecomeApiModel GetHowToBecomeApiModel()
        {
            return new HowToBecomeApiModel
            {
                EntryRouteSummary = new List<string> { "Entry route summary" },
                MoreInformation = new MoreInformationApiModel
                {
                    CareerTips = new List<string> { "Career tip 1", "Career tip 2" },
                    FurtherInformation = new List<string> { "Further info 1", "Further info 2" },
                    ProfessionalAndIndustryBodies = new List<string> { "Professional body 1", "Professional body 2" },
                    Registrations = new List<string> { "Registration 1", "Registration 2" },
                },
                EntryRoutes = new EntryRoutesApiModel
                {
                    University = new CommonRouteApiModel
                    {
                        FurtherInformation = new List<string> { "Further uni info 1", "Further uni info 2" },
                        AdditionalInformation = new List<string> { "Additional info 1", "Additional info 2" },
                        EntryRequirementPreface = "Preface",
                        EntryRequirements = new List<string> { "Entry Requirement 1", "Entry Requirement 2" },
                        RelevantSubjects = new List<string> { "Relevant subject 1", "Relevant subject 2" },
                    },
                    Work = new List<string> { "Work 1", "Work 2" },
                },
            };
        }

        private CareerPathAndProgressionApiModel GetCareerPathAndProgressionApiModel()
        {
            return new CareerPathAndProgressionApiModel
            {
                CareerPathAndProgression = new List<string> { "Career Path 1", "Career Path 2" },
            };
        }
    }
}