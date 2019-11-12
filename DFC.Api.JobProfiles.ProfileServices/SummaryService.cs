using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public class SummaryService : ISummaryService
    {
        private readonly ICosmosRepository<SummaryDataModel> repository;
        private readonly IMapper mapper;

        public SummaryService(ICosmosRepository<SummaryDataModel> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SummaryApiModel>> GetSummaryList(string requestUrl)
        {
            var dataModels = await repository.GetData(
                    s => new SummaryDataModel
                    {
                        CanonicalName = s.CanonicalName,
                        BreadcrumbTitle = s.BreadcrumbTitle,
                        LastReviewed = s.LastReviewed,
                    },
                    null)
                .ConfigureAwait(false);

            if (dataModels is null)
            {
                return null;
            }

            var viewModels = mapper.Map<IEnumerable<SummaryApiModel>>(dataModels).ToList();
            viewModels.ForEach(v => v.Url = $"{requestUrl}/{v.Url}");

            return viewModels;
        }
    }
}