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
    public class FurtherRouteInfoResolver : IValueResolver<JobProfileHowToBecomeResponse, CommonRouteApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            CommonRouteApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            RouteName routeName = (RouteName)context.Items["RouteName"];
            //string furtherRouteInfo = null;
            var furtherRouteInfo = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();


            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                switch (routeName)
                {
                    case RouteName.Apprenticeship:
                        furtherRouteInfo.AddRange(dataTranslator.Translate(responseData.ApprenticeshipFurtherRoutesInfo.Html));
                        break;
                    case RouteName.College:
                        furtherRouteInfo.AddRange(dataTranslator.Translate(responseData.CollegeFurtherRouteInfo.Html));
                        break;
                    case RouteName.University:
                        furtherRouteInfo.AddRange(dataTranslator.Translate(responseData.UniversityFurtherRouteInfo.Html));
                        break;
                }
            }

            return furtherRouteInfo;
        }
    }
}
