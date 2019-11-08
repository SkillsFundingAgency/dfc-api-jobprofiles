using DFC.Api.JobProfiles.Data.ApiModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.Api.JobProfiles.Data.DataModels;

namespace DFC.Api.JobProfiles.Repository.CosmosDb
{
    public interface ICosmosRepository
    {
        Task<bool> PingAsync();

        Task<IEnumerable<SummaryDataModel>> GetSummaryListAsync();

        Task<JobProfileApiModel>
    }
}