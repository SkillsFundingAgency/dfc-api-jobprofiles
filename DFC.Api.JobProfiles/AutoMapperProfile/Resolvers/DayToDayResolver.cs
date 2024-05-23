using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.HtmlToDataTranslator.Services;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class DayToDayResolver : IValueResolver<JobProfileWhatYoullDoResponse, WhatYouWillDoApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileWhatYoullDoResponse source,
            WhatYouWillDoApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var daytoDay = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileWhatYoullDo.IsAny())
            {
                var responseData = source.JobProfileWhatYoullDo.FirstOrDefault();

                if (responseData.Daytodaytasks.Html != null)
                {
                    daytoDay = dataTranslator.Translate(responseData.Daytodaytasks.Html);
                    return daytoDay;
                }
            }

            return daytoDay;
        }
    }
}
