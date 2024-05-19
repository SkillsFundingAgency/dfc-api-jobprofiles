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
using FluentNHibernate.Conventions;

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

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                switch (routeName)
                {
                    case RouteName.Apprenticeship:
                        furtherRouteInfo.Add(responseData.ApprenticeshipFurtherRoutesInfo.Html);
                        break;
                    case RouteName.College:
                        furtherRouteInfo.Add(responseData.ApprenticeshipFurtherRoutesInfo.Html);
                        break;
                    case RouteName.University:
                        furtherRouteInfo.Add(responseData.ApprenticeshipFurtherRoutesInfo.Html);
                        break;
                }
            }

            return furtherRouteInfo;
        }
    }
}
