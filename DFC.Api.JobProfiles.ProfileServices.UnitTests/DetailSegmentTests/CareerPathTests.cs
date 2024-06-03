using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DFC.Api.JobProfiles.ProfileServices.UnitTests.DetailSegmentTests
{
    public class CareerPathTests
    {
        [Fact]
        public async Task GetRelatedCareersDataSuccess()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();

            var canonicalName = "biochemist";
            var status = "PUBLISHED";


            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileCareerPathAndProgressionResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(expectedResult);

            //Act
            var response = await profileDataService.GetCareerPathSegmentAsync(canonicalName, status);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileCareerPathAndProgressionResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(response);
            response.Should().BeOfType(typeof(CareerPathAndProgressionApiModel));
        }

        [Fact]
        public async Task GetRelatedCareersDataNoSuccessAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();

            var canonicalName = "biochemist";
            var status = "PUBLISHED";

            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileCareerPathAndProgressionResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(new JobProfileCareerPathAndProgressionResponse());

            //Act
            var response = await profileDataService.GetCareerPathSegmentAsync(canonicalName, status);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileCareerPathAndProgressionResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            response.Should().BeOfType(typeof(CareerPathAndProgressionApiModel));
        }

        private static JobProfileCareerPathAndProgressionResponse GetExpectedData()
        {
            var expectedResult = new JobProfileCareerPathAndProgressionResponse();

            var list = new List<JobProfileCareerPathAndProgression>
            {
                new JobProfileCareerPathAndProgression
                {
                    DisplayText = "biochemist",
                    Content = new JobProileCareerPath()
                    {
                        Html = "ExampleTestHtmlStringBiochemist",
                    },
                },
            };

            expectedResult.JobProileCareerPath = list;
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
