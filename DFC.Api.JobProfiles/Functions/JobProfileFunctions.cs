using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Extensions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Functions.DI.Standard.Attributes;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Functions
{
    public static class JobProfileFunctions
    {
        [Display(Name = "Get Summary List of Job Profiles", Description = "Retrieves a summary list of all Job Profiles")]
        [FunctionName("job-profiles")]
        [ProducesResponseType(typeof(SummaryApiModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Summary list of Job Profiles found.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Summary list of Job Profiles  does not exist", ShowSchema = false)]
        public static async Task<IActionResult> GetSummaryList(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest request,
            [Inject] ISummaryService summaryService)
        {
            var viewModels = await summaryService.GetSummaryList(request.HttpContext.Request.GetEncodedUrl()).ConfigureAwait(false);
            if (viewModels is null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(viewModels);
        }

        [Display(Name = "Get Job Profile Detail", Description = "Retrieves details of a specific Job Profile")]
        [FunctionName("job-profiles-detail")]
        [ProducesResponseType(typeof(JobProfileApiModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Job Profile found.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Job Profile does not exist", ShowSchema = false)]
        public static async Task<IActionResult> GetJobProfileDetail(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{canonicalName}")] HttpRequest request,
            string canonicalName,
            [Inject] IProfileDataService dataService,
            [Inject] ILogger log)
        {
            // Retrieve version off headers in request via APIM?
            var jobProfile = await dataService.GetJobProfile(canonicalName).ConfigureAwait(false);
            if (jobProfile is null)
            {
                log.LogWarning($"Job Profile with name {canonicalName} does not exist");
                return new NoContentResult();
            }

            jobProfile.RelatedCareers.ForEach(r => r.Url = request.GetAbsoluteUrlForRelativePath(r.Url));
            jobProfile.Url = request.GetAbsoluteUrlForRelativePath(jobProfile.Url);

            return new OkObjectResult(jobProfile);
        }
    }
}