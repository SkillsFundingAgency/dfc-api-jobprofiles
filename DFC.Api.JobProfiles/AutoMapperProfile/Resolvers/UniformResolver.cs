using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class UniformResolver : FormatContentService, IValueResolver<JobProfileWhatYoullDoResponse, WorkingEnvironmentApiModel, string>
    {
        public string Resolve(
            JobProfileWhatYoullDoResponse source,
            WorkingEnvironmentApiModel destination,
            string destMember,
            ResolutionContext context)
        {
            var wordList = new List<string>();

            if (source.JobProfileWhatYoullDo.IsAny())
            {
                var responseData = source.JobProfileWhatYoullDo.FirstOrDefault();

                if (responseData != null && responseData.RelatedUniforms.ContentItems.IsAny())
                {
                    foreach (var contentItem in responseData.RelatedUniforms.ContentItems)
                    {
                        wordList.Add(contentItem.Description);
                    }

                    return Convert(wordList);
                }
            }

            return string.Empty;
        }

        public string Convert(IEnumerable<string> wordList)
        {
            const string UniformLeadingText = "You may need to wear";
            const string UniformConjunction = "and";

            return GetParagraphText(UniformLeadingText, wordList, UniformConjunction);
        }
    }
}
