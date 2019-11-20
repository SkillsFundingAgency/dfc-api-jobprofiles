using Microsoft.AspNetCore.Http;

namespace DFC.Api.JobProfiles.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetAbsoluteUrlForRelativePath(this HttpRequest request, string relativePath = null)
        {
            request.Headers.TryGetValue("X-Forwarded-APIM-Url", out var apimUrl);

            var trimmedRelativePath = relativePath?.TrimStart('/');

            if (string.IsNullOrEmpty(apimUrl))
            {
                return $"{request.Scheme}://{request.Host}{request.Path}/{trimmedRelativePath}";
            }

            return $"{apimUrl.ToString().TrimEnd('/')}/{trimmedRelativePath}";
        }
    }
}