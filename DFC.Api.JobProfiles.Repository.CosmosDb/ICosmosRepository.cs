using DFC.Api.JobProfiles.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Repository.CosmosDb
{
    public interface ICosmosRepository
    {
        Task<bool> PingAsync();

        Task<IEnumerable<SummaryDataModel>> GetSummaryListAsync();
    }
}