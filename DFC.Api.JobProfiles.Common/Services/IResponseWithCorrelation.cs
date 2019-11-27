using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DFC.Api.JobProfiles.Common.Services
{
    public interface IResponseWithCorrelation
    {
        IActionResult ResponseWithCorrelationId(HttpStatusCode statusCode);

        IActionResult ResponseObjectWithCorrelationId(object value);
    }
}