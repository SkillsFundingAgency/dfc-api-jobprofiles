using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.UnitTests
{
    public class JobProfileFunctionsSummaryListTests
    {
        private readonly HttpRequest httpRequest;
        private readonly ISummaryService fakeSummaryService;

        public JobProfileFunctionsSummaryListTests()
        {
            httpRequest = A.Fake<HttpRequest>();
            fakeSummaryService = A.Fake<ISummaryService>();
        }

        [Fact]
        public async Task GetSummaryListReturnsOKAndViewModelsWhenReturnedFromSummaryService()
        {
            var expectedModels = GetSummaryApiModels();
            A.CallTo(() => fakeSummaryService.GetSummaryList(A<string>.Ignored)).Returns(expectedModels);

            var result = await JobProfileFunctions.GetSummaryList(httpRequest, fakeSummaryService).ConfigureAwait(false);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            okResult.Value.Should().BeEquivalentTo(expectedModels);
        }

        [Fact]
        public async Task GetSummaryListReturnsNoContentResultWhenNullReturnedFromSummaryService()
        {
            A.CallTo(() => fakeSummaryService.GetSummaryList(A<string>.Ignored)).Returns((IEnumerable<SummaryApiModel>)null);

            var result = await JobProfileFunctions.GetSummaryList(httpRequest, fakeSummaryService).ConfigureAwait(false);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        private IList<SummaryApiModel> GetSummaryApiModels()
        {
            return new List<SummaryApiModel>
            {
                new SummaryApiModel
                {
                    Title = "JobTitle1",
                    Url = "/job1",
                    LastUpdated = DateTime.UtcNow,
                },
                new SummaryApiModel
                {
                    Title = "JobTitle2",
                    Url = "/job2",
                    LastUpdated = DateTime.UtcNow.AddDays(-1),
                },
            };
        }
    }
}