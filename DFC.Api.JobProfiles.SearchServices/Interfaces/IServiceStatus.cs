using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface IServiceStatus
    {
        Task<ServiceStatus> GetCurrentStatusAsync();
    }
}
