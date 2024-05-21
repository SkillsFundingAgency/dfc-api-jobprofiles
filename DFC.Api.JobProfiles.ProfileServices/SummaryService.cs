using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
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
        private readonly IMapper mapper;
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;

        public SummaryService(IMapper mapper, ISharedContentRedisInterface sharedContentRedisInterface)
        {
            this.mapper = mapper;
            this.sharedContentRedisInterface = sharedContentRedisInterface;
        }

        public async Task<IList<SummaryApiModel>> GetSummaryList(string requestUrl)
        {
            var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileApiSummaryResponse>(ApplicationKeys.JobProfileApiSummaryAll, "PUBLISHED");
            if (response.JobProfileSummary != null)
            {
                var listResponse = response.JobProfileSummary.ToList();
                var viewModels = mapper.Map<List<SummaryApiModel>>(listResponse);

                viewModels.ForEach(v => v.Url = $"{requestUrl}{v.Url.TrimStart('/')}");

                return viewModels;
            }
            else
            {
                return null;
            }
        }
    }
}