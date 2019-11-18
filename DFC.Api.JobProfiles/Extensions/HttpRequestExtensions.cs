using Microsoft.AspNetCore.Http;

namespace DFC.Api.JobProfiles.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetAbsoluteUrlForRelativePath(this HttpRequest request, string relativePath = null)
        {
            request.Headers.TryGetValue("X-Original-Url", out var apimUrl);

            return string.IsNullOrEmpty(apimUrl) ? $"{request.Scheme}://{request.Host}{request.Path}/{relativePath?.TrimStart('/')}" : $"{apimUrl.ToString().TrimEnd('/')}/{relativePath?.TrimStart('/')}";
        }
    }
}