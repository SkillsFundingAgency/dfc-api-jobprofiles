using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

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

        public static void LogRequestHeaders(this HttpRequest request, ILogger log)
        {
            var message = new StringBuilder();

            foreach (var key in request.Headers.Keys)
            {
                message.AppendLine($"Request Header Key: '{key}', Value: '{request.Headers[key]}'");
            }

            log.LogError(message.ToString());
        }
    }
}