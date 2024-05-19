using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class RegistrationResolver : IValueResolver<JobProfileHowToBecomeResponse, MoreInformationApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            MoreInformationApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var registrations = new List<string>();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                if (responseData.RelatedRegistrations.ContentItems.IsAny())
                {
                    foreach (var item in responseData.RelatedRegistrations.ContentItems)
                    {
                        registrations.Add(string.Concat(item.DisplayText, item.Info.Html));
                    }
                }
            }

            return registrations;
        }
    }
}
