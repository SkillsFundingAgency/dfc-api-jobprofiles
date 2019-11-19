using Microsoft.AspNetCore.Http;

namespace DFC.Api.JobProfiles.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetAbsoluteUrlForRelativePath(this HttpRequest request, string relativePath = null)
        {
            request.Headers.TryGetValue("X-Original-Url", out var apimUrl);
            var trimmedRelativePath = relativePath?.TrimStart('/');

            if (string.IsNullOrEmpty(apimUrl))
            {
                return $"{request.Scheme}://{request.Host}/job-profiles/{trimmedRelativePath}";
            }

            return $"{apimUrl.ToString().TrimEnd('/')}/{trimmedRelativePath}";
        }
    }
}