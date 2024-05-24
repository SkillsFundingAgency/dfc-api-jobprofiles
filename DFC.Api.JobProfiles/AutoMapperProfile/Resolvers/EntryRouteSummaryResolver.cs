using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.HtmlToDataTranslator.Services;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    internal class EntryRouteSummaryResolver : IValueResolver<JobProfileHowToBecomeResponse, HowToBecomeApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            HowToBecomeApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var entrySummary = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                if (responseData.EntryRoutes.Html != null)
                {
                    entrySummary = dataTranslator.Translate(responseData.EntryRoutes.Html);
                    return entrySummary;
                }
            }

            return entrySummary;
        }
    }
}
