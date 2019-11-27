using DFC.Api.JobProfiles.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DFC.Api.JobProfiles.Common.Services
{
    public class ResponseWithCorrelation : IResponseWithCorrelation
    {
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ResponseWithCorrelation(ICorrelationIdProvider correlationIdProvider, IHttpContextAccessor httpContextAccessor)
        {
            this.correlationIdProvider = correlationIdProvider;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult ResponseWithCorrelationId(HttpStatusCode statusCode)
        {
            AddCorrelationId();
            return new StatusCodeResult((int)statusCode);
        }

        public IActionResult ResponseObjectWithCorrelationId(object value)
        {
            AddCorrelationId();
            return new OkObjectResult(value);
        }

        private void AddCorrelationId()
        {
            httpContextAccessor.HttpContext.Response.Headers.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
        }
    }
}