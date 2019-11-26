using DFC.Api.JobProfiles.Data.ApiModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public interface ISummaryService
    {
        Task<IList<SummaryApiModel>> GetSummaryList(string requestUrl);
    }
}