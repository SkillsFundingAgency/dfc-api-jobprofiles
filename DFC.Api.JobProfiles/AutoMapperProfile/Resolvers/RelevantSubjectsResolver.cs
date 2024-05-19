using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Enums;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;

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

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                switch (routeName)
                {
                    case RouteName.Apprenticeship:
                        relevantSubjects.Add(responseData.ApprenticeshipRelevantSubjects.Html);
                        break;
                    case RouteName.College:
                        relevantSubjects.Add(responseData.CollegeRelevantSubjects.Html);
                        break;
                    case RouteName.University:
                        relevantSubjects.Add(responseData.UniversityRelevantSubjects.Html);
                        break;
                }
            }

            return relevantSubjects;
        }
    }
}
