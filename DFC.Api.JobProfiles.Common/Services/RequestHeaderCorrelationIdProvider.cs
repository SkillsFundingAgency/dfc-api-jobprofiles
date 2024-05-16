using DFC.Api.JobProfiles.Common.Constants;
using DFC.Common.SharedContent.Pkg.Netcore.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using System;

namespace DFC.Api.JobProfiles.Common.Services
{
    public class RequestHeaderCorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IFunctionContextAccessor functionContextAccessor;

        public RequestHeaderCorrelationIdProvider(IHttpContextAccessor httpContextAccessor, IFunctionContextAccessor functionContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.functionContextAccessor = functionContextAccessor;
        }

        public string CorrelationId => !string.IsNullOrWhiteSpace(functionContextAccessor.FunctionContext.GetHttpContext().Request.Headers[HeaderName.RequestId].ToString())
            ? httpContextAccessor.HttpContext.Request.Headers[HeaderName.RequestId].ToString()
            : Guid.NewGuid().ToString();
    }
}