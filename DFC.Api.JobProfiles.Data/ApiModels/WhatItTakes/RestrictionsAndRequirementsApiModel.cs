using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes
{
    public class RestrictionsAndRequirementsApiModel
    {
        public List<string> RelatedRestrictions { get; set; }

        public List<string> OtherRequirements { get; set; }
    }
}