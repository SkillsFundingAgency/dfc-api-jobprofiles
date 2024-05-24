﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.HtmlToDataTranslator.Services;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class OtherRequirementsResolver : IValueResolver<JobProfileSkillsResponse, RestrictionsAndRequirementsApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileSkillsResponse source,
            RestrictionsAndRequirementsApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var otherRequirement = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileSkills.IsAny())
            {
                var responseData = source.JobProfileSkills.FirstOrDefault();

                if (responseData.Otherrequirements.Html != null)
                {
                    otherRequirement = dataTranslator.Translate(responseData.Otherrequirements.Html);
                    return otherRequirement;
                }
            }

            return otherRequirement;
        }
    }
}
