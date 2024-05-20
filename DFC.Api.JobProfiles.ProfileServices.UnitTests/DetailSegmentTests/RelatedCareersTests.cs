using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
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
    public class RelatedCareersTests
    {
        [Fact]
        public async Task GetRelatedCareersDataSuccess()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(repository, logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();

            var canonicalName = "biochemist";
            var filter = "PUBLISHED";



            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<RelatedCareersResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(expectedResult);

            //Act
            var response = await profileDataService.GetRelatedCareersSegmentAsync(canonicalName, filter);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<RelatedCareersResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.NotNull(response);
            response.Should().BeOfType(typeof(List<RelatedCareerApiModel>));
        }

        [Fact]
        public async Task GetRelatedCareersDataNoSuccessAsync()
        {
            //Arrange
            var repository = A.Fake<ICosmosRepository<SegmentDataModel>>();
            var mapper = A.Fake<IMapper>();
            var logger = A.Fake<ILogger>();
            var sharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

            var profileDataService = new ProfileDataService(repository, logger, mapper, sharedContentRedisInterface);
            var expectedResult = GetExpectedData();

            var canonicalName = "biochemist";
            var filter = "PUBLISHED";


            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<RelatedCareersResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).Returns(new RelatedCareersResponse());

            //Act
            var response = await profileDataService.GetRelatedCareersSegmentAsync(canonicalName, filter);

            //Assert
            A.CallTo(() => sharedContentRedisInterface.GetDataAsyncWithExpiry<RelatedCareersResponse>(A<string>.Ignored, A<string>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();
            response.Should().BeOfType(typeof(List<RelatedCareerApiModel>));
        }

        private static RelatedCareersResponse GetExpectedData()
        {
            var expectedResult = new RelatedCareersResponse();

            var list = new List<JobProfileRelatedCareers>
           {
               new JobProfileRelatedCareers
               {
                   RelatedCareerProfiles = new RelatedCareers
                   {
                       ContentItems = new List<RelatedCareersContentItems>
                       {
                           new RelatedCareersContentItems()
                           {
                               DisplayText = "LabTech",
                               ContentItemId = "12345",
                               GraphSync = new ()
                               {
                                   NodeId = "123",
                               },
                               PageLocation = new ()
                               {
                                   DefaultPageForLocation = true,
                                   FullUrl = "/labtech",
                                   UrlName = "labtech",
                               },
                           },
                       },
                   },
                   DisplayText = "biochemist",
                   PageLocation = new ()
                   {
                       DefaultPageForLocation = false,
                       FullUrl = "/biochemist",
                       UrlName = "biochemist",
                   },
               },
           };
            expectedResult.JobProfileRelatedCareers = list;
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
