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
using Microsoft.AspNetCore.Routing;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class EntryRequirementsPrefaceResolver : IValueResolver<JobProfileHowToBecomeResponse, CommonRouteApiModel, string>
    {
        public string Resolve(
            JobProfileHowToBecomeResponse source,
            CommonRouteApiModel destination,
            string destMember,
            ResolutionContext context)
        {
            RouteName routeName = (RouteName)context.Items["RouteName"];
            string entryRequirements = null;

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                switch (routeName)
                {
                    case RouteName.Apprenticeship:
                        entryRequirements = responseData.ApprenticeshipEntryRequirements.ContentItems.FirstOrDefault()?.DisplayText;
                        break;
                    case RouteName.College:
                        entryRequirements = responseData.CollegeEntryRequirements.ContentItems.FirstOrDefault()?.DisplayText;
                        break;
                    case RouteName.University:
                        entryRequirements = responseData.UniversityEntryRequirements.ContentItems.FirstOrDefault()?.DisplayText;
                        break;
                }
            }

            return entryRequirements;
        }
    }
}
