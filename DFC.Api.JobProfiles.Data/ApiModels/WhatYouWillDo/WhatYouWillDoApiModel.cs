using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo
{
    public class WhatYouWillDoApiModel
    {
        [Example(Description = "You could work for a variety of businesses and public sector organisations.")]
        public List<string> WYDDayToDayTasks { get; set; }

        public WorkingEnvironmentApiModel WorkingEnvironment { get; set; }
    }
}