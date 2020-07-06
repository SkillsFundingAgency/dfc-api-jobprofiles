using DFC.Api.JobProfiles.Data.ApiModels;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public interface IProfileDataService
    {
        Task<JobProfileApiModel> GetJobProfile(string profileName);

        Task<bool> PingAsync();
    }
}