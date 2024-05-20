using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
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
    public class TasksTests
    {
        [Fact]
        public async Task GetTasksValidInputAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(repository, logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();

            var canonicalName = "bookmaker";
            var filter = "PUBLISHED";

            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileWhatYoullDoResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(expectedResult);

            //Act
            var response = await profileDataService.GetTasksSegmentAsync(canonicalName, filter);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileWhatYoullDoResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(response);
            response.Should().BeOfType(typeof(WhatYouWillDoApiModel));
        }

        [Fact]
        public async Task GetTasksInvalidInputAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(repository, logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();
            var canonicalName = "bookmaker";
            var filter = "PUBLISHED";

            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileWhatYoullDoResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(new JobProfileWhatYoullDoResponse());

            //Act
            var response = await profileDataService.GetTasksSegmentAsync(canonicalName, filter);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileWhatYoullDoResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            response.Should().BeOfType(typeof(WhatYouWillDoApiModel));
        }

        private static JobProfileWhatYoullDoResponse GetExpectedData()
        {
            var expectedResult = new JobProfileWhatYoullDoResponse();

            var contentItemWYD = new ContentItemWYD
            {
                Description = string.Empty,
            };

            var contentItemWYDList = new List<ContentItemWYD> { contentItemWYD };

            var list = new List<JobProfileWhatYoullDo>
            {
                new JobProfileWhatYoullDo
                {
                    DisplayText = "Bookmaker",
                    Daytodaytasks = new Daytodaytasks { Html = string.Empty },
                    RelatedEnvironments = new RelatedEnvironments { ContentItems = contentItemWYDList },
                    RelatedLocations = new RelatedLocations { ContentItems = contentItemWYDList },
                    RelatedUniforms = new RelatedUniforms { ContentItems = contentItemWYDList },
                },
            };

            expectedResult.JobProfileWhatYoullDo = list;
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
