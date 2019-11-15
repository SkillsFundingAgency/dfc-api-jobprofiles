using Microsoft.AspNetCore.Http;

namespace DFC.Api.JobProfiles.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetAbsoluteUrlForRelativePath(this HttpRequest request, string relativePath)
        {
            var contextRequest = request.HttpContext.Request;
            return $"{contextRequest.Scheme}://{contextRequest.Host}/{relativePath.TrimStart('/')}";
        }
    }
}