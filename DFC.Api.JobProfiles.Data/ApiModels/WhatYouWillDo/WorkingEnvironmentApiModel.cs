using DFC.Swagger.Standard.Annotations;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo
{
    public class WorkingEnvironmentApiModel
    {
        [Example(Description = "You could work from home, in an office or at a client's business.")]
        public string Location { get; set; }

        [Example(Description = "Environment information")]
        public string Environment { get; set; }

        [Example(Description = "Uniform is not required")]
        public string Uniform { get; set; }
    }
}