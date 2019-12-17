using DFC.Swagger.Standard.Annotations;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes
{
    public class RelatedSkillsApiModel
    {
        [Example(Description = "Knowledge")]
        public string Description { get; set; }

        [Example(Description = "Knowledge")]
        public string ONetAttributeType { get; set; }

        [Example(Description = "5.06")]
        public string ONetRank { get; set; }

        [Example(Description = "2.C.3.a")]
        public string ONetElementId { get; set; }
    }
}