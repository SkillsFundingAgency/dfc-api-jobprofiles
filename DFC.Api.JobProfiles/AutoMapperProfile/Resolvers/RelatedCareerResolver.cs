using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class RelatedCareerResolver : IValueResolver<JobProfileRelatedCareers, JobProfileApiModel, List<RelatedCareerApiModel>>
    {
        public List<RelatedCareerApiModel> Resolve(
            JobProfileRelatedCareers source,
            JobProfileApiModel destination,
            List<RelatedCareerApiModel>
            destMember, ResolutionContext context)
        {
            var segmentData = new List<RelatedCareerApiModel>();

            if (source.RelatedCareerProfiles.ContentItems?.Count() > 0)
            {
                var responseData = source.RelatedCareerProfiles;

                foreach (var career in responseData.ContentItems)
                {
                    segmentData.Add(new RelatedCareerApiModel
                    {
                        Url = career.PageLocation.FullUrl,
                        Title = career.DisplayText,
                    });
                }
            }

            return segmentData;
        }
    }
}
