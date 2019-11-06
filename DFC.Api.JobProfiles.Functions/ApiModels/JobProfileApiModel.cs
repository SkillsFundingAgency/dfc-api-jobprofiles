using DFC.Api.JobProfiles.Functions.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Functions.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Functions.ApiModels.Overview;
using DFC.Api.JobProfiles.Functions.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Functions.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Functions.ApiModels.WhatYouWillDo;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Functions.ApiModels
{
    public class JobProfileApiModel : OverviewApiModel
    {
        public HowToBecomeApiModel HowToBecome { get; set; }

        public WhatItTakesApiModel WhatItTakes { get; set; }

        public WhatYouWillDoApiModel WhatYouWillDo { get; set; }

        public CareerPathAndProgressionApiModel CareerPathAndProgression { get; set; }

        public List<RelatedCareerApiModel> RelatedCareers { get; set; }
    }
}