using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.IntegrationTests.Model;
using DFC.Api.JobProfiles.IntegrationTests.Support;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Test.Create
{
    public class SummaryTest : Hook
    {
        [Test]
        public async Task ResponseCode200()
        {
            Response<List<JobProfileSummaryAPIResponse>> authorisedAPIResponseWithContent = await CommonAction.ExecuteGetRequest<List<JobProfileSummaryAPIResponse>>(Settings.APIConfig.EndpointBaseUrl.ProfileSummary);
            Assert.AreEqual(HttpStatusCode.OK, authorisedAPIResponseWithContent.HttpStatusCode);
        }

        [Test]
        public async Task ResponseCode401()
        {
            Response<List<JobProfileSummaryAPIResponse>> unauthorisedAPIResponse = await CommonAction.ExecuteGetRequest<List<JobProfileSummaryAPIResponse>>(Settings.APIConfig.EndpointBaseUrl.ProfileSummary, false);
            Assert.AreEqual(HttpStatusCode.Unauthorized, unauthorisedAPIResponse.HttpStatusCode);
        }
    }
}