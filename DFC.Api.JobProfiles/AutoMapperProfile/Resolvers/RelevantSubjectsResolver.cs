using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Enums;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.HtmlToDataTranslator.Services;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class RelevantSubjectsResolver : IValueResolver<JobProfileHowToBecomeResponse, CommonRouteApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            CommonRouteApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            RouteName routeName = (RouteName)context.Items["RouteName"];
            List<string> relevantSubjects = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                switch (routeName)
                {
                    case RouteName.Apprenticeship:
                        relevantSubjects.AddRange(dataTranslator.Translate(responseData.ApprenticeshipRelevantSubjects.Html));
                        break;
                    case RouteName.College:
                        relevantSubjects.AddRange(dataTranslator.Translate(responseData.CollegeRelevantSubjects.Html));
                        break;
                    case RouteName.University:
                        relevantSubjects.AddRange(dataTranslator.Translate(responseData.UniversityRelevantSubjects.Html));
                        break;
                }
            }

            return relevantSubjects;
        }
    }
}
