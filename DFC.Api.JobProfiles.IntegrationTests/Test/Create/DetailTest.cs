using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.IntegrationTests.Model;
using DFC.Api.JobProfiles.IntegrationTests.Support;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Test.Create
{
    public class DetailTest : Hook
    {

        [Test]
        public async Task ResponseCode200()
        {
            Response<JobProfileAPIResponse> authorisedAPIResponseWithContent = await CommonAction.ExecuteGetRequest<JobProfileAPIResponse>(Settings.APIConfig.EndpointBaseUrl.ProfileDetail, CanonicalName);
            Assert.AreEqual(HttpStatusCode.OK, authorisedAPIResponseWithContent.HttpStatusCode);
        }

        [Test]
        public async Task ResponseCode204()
        {
            Response<JobProfileAPIResponse> authorisedAPIResponseNoContent = await CommonAction.ExecuteGetRequest<JobProfileAPIResponse>(Settings.APIConfig.EndpointBaseUrl.ProfileDetail, CommonAction.RandomString(10).ToLower());
            Assert.AreEqual(HttpStatusCode.NoContent, authorisedAPIResponseNoContent.HttpStatusCode);
        }

        [Test]
        public async Task ResponseCode401()
        {
            Response<JobProfileAPIResponse> unauthorisedAPIResponse = await CommonAction.ExecuteGetRequest<JobProfileAPIResponse>(Settings.APIConfig.EndpointBaseUrl.ProfileDetail, CanonicalName, false);
            Assert.AreEqual(HttpStatusCode.Unauthorized, unauthorisedAPIResponse.HttpStatusCode);
        }
    }
}