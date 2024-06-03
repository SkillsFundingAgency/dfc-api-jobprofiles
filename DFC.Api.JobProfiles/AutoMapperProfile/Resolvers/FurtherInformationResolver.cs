using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.HtmlToDataTranslator.Services;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class FurtherInformationResolver : IValueResolver<JobProfileHowToBecomeResponse, MoreInformationApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            MoreInformationApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var furtherInformation = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                if (responseData.FurtherInformation.Html != null)
                {
                    furtherInformation = dataTranslator.Translate(responseData.FurtherInformation.Html);
                    return furtherInformation;
                }
            }
            return furtherInformation;
        }

    }


}
