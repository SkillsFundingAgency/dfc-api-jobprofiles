using DFC.Api.JobProfiles.Data.ApiModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public interface ISummaryService
    {
        Task<IEnumerable<SummaryApiModel>> GetSummaryList(string requestUrl);
    }
}