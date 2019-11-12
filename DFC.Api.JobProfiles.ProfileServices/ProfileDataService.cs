using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.Overview;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Data.Enums;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public class ProfileDataService : IProfileDataService
    {
        private readonly ICosmosRepository<SegmentDataModel> repository;
        private readonly IMapper mapper;
        private readonly ILogger log;

        public ProfileDataService(ICosmosRepository<SegmentDataModel> repository, IMapper mapper, ILogger log)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.log = log;
        }

        public async Task<JobProfileApiModel> GetJobProfile(string profileName)
        {
            var segmentDetailModels = await repository.GetData(s => new SegmentDataModel { Segments = s.Segments, CanonicalName = s.CanonicalName }, model => model.CanonicalName == profileName.ToLowerInvariant())
                .ConfigureAwait(false);

            var result = new JobProfileApiModel();
            foreach (var segmentDetailModel in segmentDetailModels.SingleOrDefault()?.Segments)
            {
                switch (segmentDetailModel.Segment)
                {
                    case JobProfileSegment.Overview:
                        {
                            var overviewApiModel = JsonConvert.DeserializeObject<OverviewApiModel>(segmentDetailModel.Json);
                            result = mapper.Map<JobProfileApiModel>(overviewApiModel);
                            break;
                        }

                    case JobProfileSegment.HowToBecome:
                        {
                            var howToBecomeApiModel = JsonConvert.DeserializeObject<HowToBecomeApiModel>(segmentDetailModel.Json);
                            result.HowToBecome = howToBecomeApiModel;
                            break;
                        }

                    case JobProfileSegment.CareerPathsAndProgression:
                        {
                            var careerPathAndProgressionApiModel = JsonConvert.DeserializeObject<CareerPathAndProgressionApiModel>(segmentDetailModel.Json);
                            result.CareerPathAndProgression = careerPathAndProgressionApiModel;
                            break;
                        }

                    case JobProfileSegment.RelatedCareers:
                        {
                            var relatedCareerApiModels = JsonConvert.DeserializeObject<List<RelatedCareerApiModel>>(segmentDetailModel.Json);
                            result.RelatedCareers = relatedCareerApiModels;
                            break;
                        }

                    case JobProfileSegment.WhatItTakes:
                        {
                            var whatItTakesApiModel = JsonConvert.DeserializeObject<WhatItTakesApiModel>(segmentDetailModel.Json);
                            result.WhatItTakes = whatItTakesApiModel;
                            break;
                        }

                    case JobProfileSegment.WhatYouWillDo:
                        {
                            var whatYouWillDoApiModel = JsonConvert.DeserializeObject<WhatYouWillDoApiModel>(segmentDetailModel.Json);
                            result.WhatYouWillDo = whatYouWillDoApiModel;
                            break;
                        }

                    default:
                        {
                            log.LogWarning($"Unknown Segment Name ('{segmentDetailModel.Segment}') retrieved for Job Profile with name {profileName}");
                            break;
                        }
                }
            }

            return result;
        }
    }
}