using System;

namespace DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes
{
    public class OnetSkill
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ONetElementId { get; set; }

        public string Description { get; set; }
    }
}