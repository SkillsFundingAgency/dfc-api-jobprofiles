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
    public class EntryRequirementsResolver : IValueResolver<JobProfileHowToBecomeResponse, CommonRouteApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            CommonRouteApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            RouteName routeName = (RouteName)context.Items["RouteName"];
            var entryRequirements = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                switch (routeName)
                {
                    case RouteName.Apprenticeship:
                        if (responseData.RelatedApprenticeshipRequirements.ContentItems.IsAny() &&
                            responseData.RelatedApprenticeshipRequirements.ContentItems.FirstOrDefault().Info != null)
                        {
                            foreach (var item in responseData.RelatedApprenticeshipRequirements.ContentItems)
                            {
                                entryRequirements.AddRange(dataTranslator.Translate(item.Info.Html));

                            }
                        }

                        break;
                    case RouteName.College:
                        if (responseData.RelatedCollegeRequirements.ContentItems.IsAny() &&
                            responseData.RelatedCollegeRequirements.ContentItems.FirstOrDefault().Info != null)
                        {
                            foreach (var item in responseData.RelatedCollegeRequirements.ContentItems)
                            {
                                entryRequirements.AddRange(dataTranslator.Translate(item.Info.Html));

                            }
                        }

                        break;
                    case RouteName.University:
                        if (responseData.RelatedUniversityRequirements.ContentItems.IsAny() &&
                            responseData.RelatedUniversityRequirements.ContentItems.FirstOrDefault()?.Info != null)
                        {
                            foreach (var item in responseData.RelatedUniversityRequirements.ContentItems)
                            {
                                entryRequirements.AddRange(dataTranslator.Translate(item.Info.Html));

                            }
                        }

                        break;
                }
            }

            return entryRequirements;
        }
    }
}
