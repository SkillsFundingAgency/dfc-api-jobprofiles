using DFC.Api.JobProfiles.Functions.ViewModels;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Functions.DI.Standard.Attributes;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Functions.Functions
{
    public class GetSummaryListHttpTrigger
    {
        [Display(Name = "Get Summary List of Job Profiles", Description = "Retrieves a summary list of all Job Profiles")]
        [FunctionName("GetSummaryList")]
        [ProducesResponseType(typeof(SummaryViewModel), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Summary list of Job Profiles found.", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Summary list of Job Profiles  does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.UnprocessableEntity, Description = "Job Profile validation error(s).", ShowSchema = false)]
        public async Task<IActionResult> GetSummaryList(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetSummaryList")] HttpRequest request,
            [Inject] IProfileService service,
            [Inject] AutoMapper.IMapper mapper)
        {
            var dataModels = await service.GetSummaryList();
            if (dataModels is null)
            {
                return new NoContentResult();
            }

            try
            {
                var viewModels = dataModels.Select(mapper.Map<SummaryViewModel>);
                return new OkObjectResult(viewModels);
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}