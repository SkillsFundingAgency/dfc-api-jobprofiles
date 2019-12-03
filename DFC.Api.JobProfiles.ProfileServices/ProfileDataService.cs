using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
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
        private readonly ILogger log;

        public ProfileDataService(ICosmosRepository<SegmentDataModel> repository, ILogger log)
        {
            this.repository = repository;
            this.log = log;
        }

        public async Task<JobProfileApiModel> GetJobProfile(string profileName)
        {
            var segmentDetailModels = await repository.GetData(
                    s => new SegmentDataModel { Segments = s.Segments, CanonicalName = s.CanonicalName },
                    model => model.CanonicalName == profileName.ToLowerInvariant())
                 .ConfigureAwait(false);

            if (segmentDetailModels is null || !segmentDetailModels.Any())
            {
                return null;
            }

            var overviewDataModel = segmentDetailModels.FirstOrDefault()?.Segments?.FirstOrDefault(s => s.Segment == JobProfileSegment.Overview);
            var result = JsonConvert.DeserializeObject<JobProfileApiModel>(overviewDataModel?.Json ?? string.Empty) ?? new JobProfileApiModel();

            var otherSegmentDataModels = segmentDetailModels.FirstOrDefault()?.Segments?.Where(s => s.Segment != JobProfileSegment.Overview).ToList();
            if (otherSegmentDataModels == null || !otherSegmentDataModels.Any())
            {
                return result;
            }

            foreach (var segmentDetailModel in otherSegmentDataModels)
            {
                switch (segmentDetailModel.Segment)
                {
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