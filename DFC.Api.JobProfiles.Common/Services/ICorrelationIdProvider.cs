namespace DFC.Api.JobProfiles.Common.Services
{
    public interface ICorrelationIdProvider
    {
        string GetCorrelationId();
    }
}