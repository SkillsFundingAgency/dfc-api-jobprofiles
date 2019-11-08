using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes
{
    public class WhatItTakesApiModel
    {
        public string DigitalSkillsLevel { get; set; }

        public List<RelatedSkillsApiModel> Skills { get; set; }

        public RestrictionsAndRequirementsApiModel RestrictionsAndRequirements { get; set; }
    }
}