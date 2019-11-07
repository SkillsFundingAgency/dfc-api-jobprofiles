using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Functions.ApiModels.WhatItTakes
{
    public class SkillsAndKnowledgeApiModel
    {
        public string DigitalSkillsLevel { get; set; }

        public List<RelatedSkillsApiModel> RelatedSkills { get; set; }
    }
}