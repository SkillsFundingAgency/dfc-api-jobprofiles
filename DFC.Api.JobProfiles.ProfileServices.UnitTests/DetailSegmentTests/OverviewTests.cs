using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DFC.Api.JobProfiles.ProfileServices.UnitTests.DetailSegmentTests
{
    public class OverviewTests
    {
        private const string CanonicalName = "auditor";

        [Fact]
        public async Task GetOverviewValidInputAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();

            var canonicalName = "auditor";

            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfilesOverviewResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(expectedResult);

            //Act
            var response = await profileDataService.GetOverviewSegment(CanonicalName, "PUBLISHED");

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfilesOverviewResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            response.Should().BeOfType(typeof(JobProfileApiModel));
        }

        [Fact]
        public async Task GetOverviewWithInvalidInputAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();

            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfilesOverviewResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(new JobProfilesOverviewResponse());

            //Act
            var response = await profileDataService.GetOverviewSegment(CanonicalName, "PUBLISHED");

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfilesOverviewResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            response.Should().BeOfType(typeof(JobProfileApiModel));
        }

        private static JobProfilesOverviewResponse GetExpectedData()
        {
            var expectedResult = new JobProfilesOverviewResponse();
            var list = new List<JobProfileOverview>
            {
                new JobProfileOverview {
                    AlternativeTitle = "AlternativeTitle",
                    DisplayText = "Auditor",
                    Maximumhours = "37",
                    Minimumhours = "39",
                    WorkingHoursDetails = new WorkingHoursDetails() {ContentItems = new List<ContentItem>() {new ContentItem() {DisplayText =  "a week" } } },
                    WorkingPatternDetails = new WorkingPatternDetails() {ContentItems = new List<ContentItem>() {new ContentItem() {DisplayText = "between 8am and 6pm" } } },
                    WorkingPattern = new WorkingPattern() { ContentItems = new List<ContentItem>() {new ContentItem() {DisplayText = "working pattern" } } },
                    SalaryExperienced = "40000",
                    SalaryStarter = "30000",
                    Overview = "Overview data goes here",
                    SocCode = new SocCode()
                    {
                        ContentItems = new List<SocCodeContentItem>() { new SocCodeContentItem()
                    {
                        DisplayText = "3537",
                        SOC2020 = "2020",
                        OnetOccupationCode = "43-3031.00",
                        SOC2020extension = "2020",
                    },
                    },
                    },
                },
            };

            expectedResult.JobProfileOverview = list;
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
