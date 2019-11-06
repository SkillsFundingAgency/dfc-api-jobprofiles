using DFC.Api.JobProfiles.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public interface IProfileService
    {
        Task<IEnumerable<SummaryDataModel>> GetSummaryList();
    }
}