using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.Api.JobProfiles.Data.DataModels;

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

        public async Task<JobProfileApiModel> GetJobProfile(string profileName)
        {
            var test = await repository.
        }
    }
}