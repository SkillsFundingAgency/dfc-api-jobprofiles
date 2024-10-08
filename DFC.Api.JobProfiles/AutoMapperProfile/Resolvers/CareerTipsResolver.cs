﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.HtmlToDataTranslator.Services;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class CareerTipsResolver : IValueResolver<JobProfileHowToBecomeResponse, MoreInformationApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            MoreInformationApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var careerTips = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                if (responseData.CareerTips.Html != null)
                {
                    careerTips = dataTranslator.Translate(responseData.CareerTips.Html);
                    return careerTips;
                }
            }
            return careerTips;
        }
    }
}

