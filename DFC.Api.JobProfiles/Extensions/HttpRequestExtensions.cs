using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DFC.Api.JobProfiles.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetAbsoluteUrlForRelativePath(this HttpRequest request, ILogger log, string relativePath = null)
        {
            foreach (var key in request.Headers.Keys)
            {
                log.LogError($"Request Headers Key: '{key}', Value: '{request.Headers[key]}'");
            }
            
            request.Headers.TryGetValue("X-Original-Url", out var apimUrl);

            log.LogError($"Apim Url set to {apimUrl}");

            var trimmedRelativePath = relativePath?.TrimStart('/');

            if (string.IsNullOrEmpty(apimUrl))
            {
                var fullPath = $"{request.Scheme}://{request.Host}/job-profiles/{trimmedRelativePath}";
                log.LogError($"Request Scheme: '{request.Scheme}', RequestHost: '{request.Host}', FullPath: '{fullPath}'");
                return fullPath;
            }

            var returnPath = $"{apimUrl.ToString().TrimEnd('/')}/{trimmedRelativePath}";
            log.LogError($"Apim Url returned '{returnPath}'");
            return returnPath;
        }
    }
}
