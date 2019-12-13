using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes
{
    public class WhatItTakesApiModel
    {
        [Example(Description = "to have a thorough understanding of computer systems and applications")]
        public string DigitalSkillsLevel { get; set; }

        public List<RelatedSkillsApiModel> Skills { get; set; }

        public RestrictionsAndRequirementsApiModel RestrictionsAndRequirements { get; set; }
    }
}