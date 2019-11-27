using DFC.Api.JobProfiles.Common.Services;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
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

        public static void LogRequestHeaders(this HttpRequest request, ILogService logger)
        {
            var message = new StringBuilder();

            foreach (var key in request.Headers.Keys)
            {
                message.AppendLine($"Request Header Key: '{key}', Value: '{request.Headers[key]}'");
            }

            logger?.LogMessage(message.ToString(), SeverityLevel.Information);
        }
    }
}