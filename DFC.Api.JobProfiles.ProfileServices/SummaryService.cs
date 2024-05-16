using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Common.SharedContent.Pkg.Netcore.Constant;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public class SummaryService : ISummaryService
    {
        private readonly ICosmosRepository<SummaryDataModel> repository;
        private readonly IMapper mapper;
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;

        public SummaryService(ICosmosRepository<SummaryDataModel> repository, IMapper mapper, ISharedContentRedisInterface sharedContentRedisInterface)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.sharedContentRedisInterface = sharedContentRedisInterface;
        }

        public async Task<IList<SummaryApiModel>> GetSummaryList(string requestUrl)
        {
            var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileApiSummaryResponse>(ApplicationKeys.JobProfileApiSummaryAll, "PUBLISHED");
            var listResponse = response.JobProfileSummary.ToList();
            var viewModels = mapper.Map<List<SummaryApiModel>>(listResponse);

            if (listResponse is null)
            {
                return null;
            }

            //var viewModels = mapper.Map<List<SummaryApiModel>>(listData);
            viewModels.ForEach(v => v.Url = $"{requestUrl}{v.Url.TrimStart('/')}");

            return viewModels;
        }
    }
}