namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public enum ServiceState
    {
        Red,
        Green,
        Amber,
    }

    public class ServiceStatus
    {
        public string Name { get; set; }

        public ServiceState Status { get; set; }

        public string CheckParametersUsed { get; set; }

        public string Notes { get; set; }
    }
}