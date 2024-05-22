using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class CareerPathResolver : IValueResolver<JobProfileCareerPathAndProgressionResponse, CareerPathAndProgressionApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileCareerPathAndProgressionResponse source,
            CareerPathAndProgressionApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var careerPath = new List<string>();

            if (source != null && source.JobProileCareerPath.IsAny())
            {
                var responseData = source.JobProileCareerPath.FirstOrDefault();

                if (responseData.Content.Html != null)
                {
                    careerPath.Add(responseData.Content.Html);
                    return careerPath;
                }
            }
            return careerPath;
        }
    }
}
