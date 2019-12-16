using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes
{
    public class RestrictionsAndRequirementsApiModel
    {
        [Example(Description = "Related restriction example")]
        public List<string> RelatedRestrictions { get; set; }

        [Example(Description = "Other requirements example")]
        public List<string> OtherRequirements { get; set; }
    }
}