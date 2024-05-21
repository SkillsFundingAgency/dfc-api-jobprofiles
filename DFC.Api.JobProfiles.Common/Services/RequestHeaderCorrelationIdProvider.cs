using DFC.Api.JobProfiles.Common.Constants;
using DFC.Common.SharedContent.Pkg.Netcore.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using System;

namespace DFC.Api.JobProfiles.Common.Services
{
    public class RequestHeaderCorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly IFunctionContextAccessor functionContextAccessor;

        public RequestHeaderCorrelationIdProvider(IFunctionContextAccessor functionContextAccessor)
        {
            this.functionContextAccessor = functionContextAccessor;
        }

        public string GetCorrelationId()
        {
            return !string.IsNullOrWhiteSpace(functionContextAccessor.FunctionContext.GetHttpContext().Request.Headers[HeaderName.RequestId].ToString())
            ? functionContextAccessor.FunctionContext.GetHttpContext().Request.Headers[HeaderName.RequestId].ToString()
            : Guid.NewGuid().ToString();
        }
    }
}