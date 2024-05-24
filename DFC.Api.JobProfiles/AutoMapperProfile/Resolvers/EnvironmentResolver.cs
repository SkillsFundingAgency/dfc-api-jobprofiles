﻿using System;
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
    public class EnvironmentResolver : FormatContentService, IValueResolver<JobProfileWhatYoullDoResponse, WorkingEnvironmentApiModel, string>
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

                if (responseData != null && responseData.RelatedEnvironments.ContentItems.IsAny())
                {
                    foreach (var contentItem in responseData.RelatedEnvironments.ContentItems)
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
            const string EnvironmentLeadingText = "Your working environment may be";
            const string EnvironmentConjunction = "and";

            return GetParagraphText(EnvironmentLeadingText, wordList, EnvironmentConjunction);
        }
    }
}