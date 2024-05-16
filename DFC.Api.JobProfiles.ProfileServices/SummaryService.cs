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
            JobProfilesOverviewResponse responseTest = new JobProfilesOverviewResponse()
            {
                JobProfileOverview = new()
                {
                    new()
                    {
                        DisplayText = "Test1",
                        PageLocation = new()
                        {
                            UrlName = "TestUrl1",
                        },
                    },
                    new()
                    {
                        DisplayText = "Test2",
                        PageLocation = new()
                        {
                            UrlName = "TestUrl2",
                        },
                    },
                },
            };
            var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileApiSummaryResponse>(ApplicationKeys.JobProfileApiSummaryAll, "PUBLISHED");
            var listResponse = response.JobProfileSummary.ToList();
            var viewModels1 = mapper.Map<List<SummaryApiModel>>(listResponse);
            //var data = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfilesOverviewResponse>(ApplicationKeys.JobProfilesOverview + "/hnandra", "PUBLISHED");
            //var listData = responseTest.JobProfileOverview.ToList();

           /* var dataModels = await repository.GetData(
                    s => new SummaryDataModel
                    {
                        CanonicalName = s.CanonicalName,
                        BreadcrumbTitle = s.BreadcrumbTitle,
                        LastReviewed = s.LastReviewed,
                    },
                    s => s.IncludeInSitemap.HasValue && s.IncludeInSitemap.Value)
                .ConfigureAwait(false);*/

            if (listResponse is null)
            {
                return null;
            }

            //var viewModels = mapper.Map<List<SummaryApiModel>>(listData);
            viewModels1.ForEach(v => v.Url = $"{requestUrl}{v.Url.TrimStart('/')}");

            return viewModels1;
        }
    }
}