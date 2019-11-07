using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Functions.ApiModels.WhatYouWillDo
{
    public class WhatYouWillDoApiModel
    {
        public List<string> WYDDayToDayTasks { get; set; }

        public WorkingEnvironmentApiModel WorkingEnvironment { get; set; }
    }
}