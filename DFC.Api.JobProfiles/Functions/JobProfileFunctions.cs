using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Extensions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Functions.DI.Standard.Attributes;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Functions
{
    public static class JobProfileFunctions
    {
        [Display(Name = "Get job profiles summary", Description = "Gets a list of all published job profiles summary data, you can use this to determine updates to job profiles. This call does not support paging at this time.")]
        [FunctionName("job-profiles")]
        [ProducesResponseType(typeof(SummaryApiModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "List of all published job profiles summary data.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "No published job profiles available at this time.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public static async Task<IActionResult> GetSummaryList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "summary")] HttpRequest request,
            [Inject] ISummaryService summaryService)
        {
            var viewModels = await summaryService.GetSummaryList(request.GetAbsoluteUrlForRelativePath()).ConfigureAwait(false);
            if (viewModels is null || !viewModels.Any())
            {
                return new NoContentResult();
            }

            return new OkObjectResult(viewModels.OrderBy(jp => jp.Title));
        }

        [Display(Name = "Get job profile detail", Description = "Gets details of a specific job profile")]
        [FunctionName("job-profiles-detail")]
        [ProducesResponseType(typeof(JobProfileApiModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Job profile details.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Job profile does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public static async Task<IActionResult> GetJobProfileDetail(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{canonicalName}")] HttpRequest request,
            string canonicalName,
            [Inject] IProfileDataService dataService,
            ILogger log)
        {
            var jobProfile = await dataService.GetJobProfile(canonicalName).ConfigureAwait(false);
            if (jobProfile is null)
            {
                log.LogWarning($"Job Profile with name {canonicalName} does not exist");
                return new NoContentResult();
            }

            jobProfile.RelatedCareers.ForEach(r => r.Url = request.GetAbsoluteUrlForRelativePath(r.Url.TrimStart('/')));
            jobProfile.Url = request.GetAbsoluteUrlForRelativePath(jobProfile.Url.TrimStart('/'));

            return new OkObjectResult(jobProfile);
        }
    }
}