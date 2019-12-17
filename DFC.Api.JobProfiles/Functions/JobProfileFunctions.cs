using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Extensions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using DFC.Functions.DI.Standard.Attributes;
using DFC.Swagger.Standard.Annotations;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Functions
{
    public class JobProfileFunctions
    {
        private readonly ILogService logService;
        private readonly IResponseWithCorrelation responseWithCorrelation;

        public JobProfileFunctions(ILogService logService, IResponseWithCorrelation responseWithCorrelation)
        {
            this.logService = logService;
            this.responseWithCorrelation = responseWithCorrelation;
        }

        [Display(Name = "Get job profiles summary", Description = "Gets a list of all published job profiles summary data, you can use this to determine updates to job profiles. This call does not support paging at this time.")]
        [FunctionName("job-profiles")]
        [ProducesResponseType(typeof(SummaryApiModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "List of all published job profiles summary data.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "No published job profiles available at this time.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public async Task<IActionResult> GetSummaryList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "summary")] HttpRequest request,
            [Inject] ISummaryService summaryService)
        {
            request.LogRequestHeaders(logService);

            var viewModels = await summaryService.GetSummaryList(request.GetAbsoluteUrlForRelativePath()).ConfigureAwait(false);
            if (viewModels is null || !viewModels.Any())
            {
                return responseWithCorrelation.ResponseWithCorrelationId(HttpStatusCode.NoContent);
            }

            return responseWithCorrelation.ResponseObjectWithCorrelationId(viewModels.OrderBy(jp => jp.Title));
        }

        [Display(Name = "Get job profile detail", Description = "Gets details of a specific job profile")]
        [FunctionName("job-profiles-detail")]
        [ProducesResponseType(typeof(JobProfileApiModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Job profile details.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Job profile does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public async Task<IActionResult> GetJobProfileDetail(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{canonicalName}")] HttpRequest request,
            string canonicalName,
            [Inject] IProfileDataService dataService)
        {
            request.LogRequestHeaders(logService);

            var jobProfile = await dataService.GetJobProfile(canonicalName).ConfigureAwait(false);
            if (jobProfile is null)
            {
                logService.LogMessage($"Job Profile with name {canonicalName} does not exist", SeverityLevel.Warning);
                return responseWithCorrelation.ResponseWithCorrelationId(HttpStatusCode.NoContent);
            }

            jobProfile.RelatedCareers.ForEach(r => r.Url = request.GetAbsoluteUrlForRelativePath(r.Url.TrimStart('/')));
            jobProfile.Url = request.GetAbsoluteUrlForRelativePath(jobProfile.Url?.TrimStart('/'));

            return responseWithCorrelation.ResponseObjectWithCorrelationId(jobProfile);
        }

        [Display(Name = "Get job profile search results", Description = "Gets search results from job profiles")]
        [FunctionName("job-profiles-search")]
        [ProducesResponseType(typeof(SearchApiModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Job profile search results.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "No Job profiles meet search criteria", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public async Task<IActionResult> GetJobProfileSearchResults(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "search/{searchTerm}/{page?}/{pageSize?}")] HttpRequest request,
            [Inject] ISearchService searchService,
            string searchTerm,
            int? page,
            int? pageSize)
        {
            request.LogRequestHeaders(logService);

            page ??= 1;
            pageSize ??= 10;

            var apiModels = await searchService.GetResutsList(request.GetAbsoluteUrlForRelativePath(), searchTerm, page.Value, pageSize.Value).ConfigureAwait(false);
            if (apiModels is null || !apiModels.Any())
            {
                return responseWithCorrelation.ResponseWithCorrelationId(HttpStatusCode.NoContent);
            }

            return responseWithCorrelation.ResponseObjectWithCorrelationId(apiModels);
        }
    }
}