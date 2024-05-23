using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.HtmlToDataTranslator.Services;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class DirectApplicationResolver : IValueResolver<JobProfileHowToBecomeResponse, EntryRoutesApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            EntryRoutesApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var directApplication = new List<string>();
            HtmlAgilityPackDataTranslator dataTranslator = new HtmlAgilityPackDataTranslator();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                if (responseData.DirectApplication.Html != null)
                {
                    directApplication = dataTranslator.Translate(responseData.DirectApplication.Html);
                    return directApplication;
                }
            }

            return directApplication;
        }
    }
}
