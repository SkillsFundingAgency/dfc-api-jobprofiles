using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.Overview;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels
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