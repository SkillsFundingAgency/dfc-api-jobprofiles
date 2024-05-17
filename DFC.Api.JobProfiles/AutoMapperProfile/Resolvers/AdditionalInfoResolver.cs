/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Microsoft.AspNetCore.Routing;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class AdditionalInfoResolver : IValueResolver<JobProfileHowToBecomeResponse, CommonRouteApiModel, List<MoreInformationApiModel>>
    {
        public List<MoreInformationApiModel> Resolve(
            JobProfileHowToBecomeResponse source,
            CommonRouteApiModel destination,
        List<MoreInformationApiModel> destMember,
            ResolutionContext context)
        {
            RouteName routeName = (RouteName)context.Items["RouteName"];
            var additionalInfo = new List<AdditionalInformation>();

            if (source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                switch (routeName)
                {
                    case RouteName.Apprenticeship:
                        if (responseData.RelatedApprenticeshipLinks.ContentItems.IsAny() &&
                            responseData.RelatedApprenticeshipRequirements.ContentItems.IsAny())
                        {
                            foreach (var item in responseData.RelatedApprenticeshipLinks.ContentItems)
                            {
                                additionalInfo.Add(new AdditionalInformation
                                {
                                    Link = item.URL,
                                    Text = item.Text,
                                });
                            }
                        }

                        break;
                    case RouteName.College:
                        if (responseData.RelatedCollegeLinks.ContentItems.IsAny() &&
                            responseData.RelatedCollegeRequirements.ContentItems.IsAny())
                        {
                            foreach (var item in responseData.RelatedCollegeLinks.ContentItems)
                            {
                                additionalInfo.Add(new AdditionalInformation
                                {
                                    Link = item.URL,
                                    Text = item.Text,
                                });
                            }
                        }

                        break;
                    case RouteName.University:
                        if (responseData.RelatedUniversityLinks.ContentItems.IsAny() &&
                            responseData.RelatedApprenticeshipRequirements.ContentItems.IsAny())
                        {
                            foreach (var item in responseData.RelatedUniversityLinks.ContentItems)
                            {
                                additionalInfo.Add(new AdditionalInformation
                                {
                                    Link = item.URL,
                                    Text = item.Text,
                                });
                            }
                        }

                        break;
                }
            }

            return additionalInfo;
        }
    }
}
*/