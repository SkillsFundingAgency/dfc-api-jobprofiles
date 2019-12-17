using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Api.JobProfiles.AutoMapperProfile.ValueConverters
{
    [ExcludeFromCodeCoverage]
    public class JobProfileCategoryFormatter : IValueConverter<IEnumerable<string>, IEnumerable<JobProfileCategoryApiModel>>
    {
        public IEnumerable<JobProfileCategoryApiModel> Convert(IEnumerable<string> sourceMember, ResolutionContext context)
        {
            if (sourceMember is null || !sourceMember.Any())
            {
                return null;
            }

            var result = (from a in sourceMember
                          select new JobProfileCategoryApiModel
                          {
                              Title = a.Contains("|") ? a.Split("|")[0] : a,
                              Name = a.Contains("|") ? a.Split("|")[1] : a,
                          }).ToList();

            return result;
        }
    }
}
