using Microsoft.ApplicationInsights.DataContracts;

namespace DFC.Api.JobProfiles.Common.Services
{
    public interface ILogService
    {
        void LogMessage(string message, SeverityLevel severityLevel);
    }
}