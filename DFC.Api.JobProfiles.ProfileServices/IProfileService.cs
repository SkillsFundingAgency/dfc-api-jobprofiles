using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public interface IProfileService
    {
        Task<IEnumerable<SummaryDataModel>> GetSummaryList();

        Task<JobProfileApiModel> GetJobProfile(string profileName);
    }
}