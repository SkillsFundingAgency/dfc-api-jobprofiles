using DFC.Api.JobProfiles.Common.Constants;
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
                return $"{request.Scheme}://{request.Host}/{trimmedRelativePath}";
            }

            return $"{apimUrl.ToString().TrimEnd('/')}/{trimmedRelativePath}";
        }

        public static void LogRequestHeaders(this HttpRequest request, ILogService logger)
        {
            var message = new StringBuilder();

            request.Headers.TryGetValue(HeaderName.ApimUrl, out var apimUrl);
            message.AppendLine($"Request Header Key: '{HeaderName.ApimUrl}', Value: '{apimUrl}'");

            request.Headers.TryGetValue(HeaderName.RequestId, out var requestId);
            message.AppendLine($"Request Header Key: '{HeaderName.RequestId}', Value: '{requestId}'");

            request.Headers.TryGetValue(HeaderName.Version, out var version);
            message.AppendLine($"Request Header Key: '{HeaderName.Version}', Value: '{version}'");

            logger?.LogMessage(message.ToString(), SeverityLevel.Information);
        }
    }
}