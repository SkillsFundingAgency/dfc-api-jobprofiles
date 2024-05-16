using DFC.Api.JobProfiles.Common.Constants;
using DFC.Api.JobProfiles.Data.ContractResolver;
using DFC.Common.SharedContent.Pkg.Netcore.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using System.Net;

namespace DFC.Api.JobProfiles.Common.Services
{
    public class ResponseWithCorrelation : IResponseWithCorrelation
    {
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IFunctionContextAccessor functionContextAccessor;


        public ResponseWithCorrelation(ICorrelationIdProvider correlationIdProvider, IHttpContextAccessor httpContextAccessor, IFunctionContextAccessor functionContextAccessor)
        {
            this.correlationIdProvider = correlationIdProvider;
            this.httpContextAccessor = httpContextAccessor;
            this.functionContextAccessor = functionContextAccessor;
        }

        public IActionResult ResponseWithCorrelationId(HttpStatusCode statusCode)
        {
            AddCorrelationId();
            return new StatusCodeResult((int)statusCode);
        }

        public IActionResult ResponseObjectWithCorrelationId(object value)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new OrderedContractResolver() };
            var orderedModel = JsonConvert.SerializeObject(value, Formatting.Indented, settings);

            AddCorrelationId();

            var test = JsonConvert.DeserializeObject(orderedModel);

            return new OkObjectResult(test);
        }

        private void AddCorrelationId()
        {
            functionContextAccessor.FunctionContext.GetHttpContext().Response.Headers.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
            //httpContextAccessor.HttpContext.Response.Headers.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
        }
    }
}