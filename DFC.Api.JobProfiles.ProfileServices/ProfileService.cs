using DFC.Api.JobProfiles.Data.Models;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly ICosmosRepository repository;

        public ProfileService(ICosmosRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<SummaryDataModel>> GetSummaryList()
        {
            return await repository.GetSummaryListAsync().ConfigureAwait(false);
        }
    }
}