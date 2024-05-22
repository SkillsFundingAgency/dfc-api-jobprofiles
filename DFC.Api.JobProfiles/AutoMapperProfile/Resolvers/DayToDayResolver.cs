using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;

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

            if (source != null && source.JobProfileWhatYoullDo.IsAny())
            {
                var responseData = source.JobProfileWhatYoullDo.FirstOrDefault();

                if (responseData.Daytodaytasks.Html != null)
                {
                    daytoDay.Add(responseData.Daytodaytasks.Html);
                    return daytoDay;
                }
            }
            return daytoDay;
        }
    }
}
