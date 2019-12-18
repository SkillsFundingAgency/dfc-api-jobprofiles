using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.Api.JobProfiles.AutoMapperProfile.ValueConverters
{
    [ExcludeFromCodeCoverage]
    public class JobProfileCategoryConverter : IValueConverter<IEnumerable<string>, IEnumerable<JobProfileCategoryApiModel>>
    {
        public IEnumerable<JobProfileCategoryApiModel> Convert(IEnumerable<string> sourceMember, ResolutionContext context)
        {
            const string FieldSeparator = "|";

            if (sourceMember is null || !sourceMember.Any())
            {
                return null;
            }

            var result = (from a in sourceMember
                          where !string.IsNullOrWhiteSpace(a)
                          select new JobProfileCategoryApiModel
                          {
                              Title = a.Contains(FieldSeparator, StringComparison.OrdinalIgnoreCase) ? a.Split(FieldSeparator)[0] : a,
                              Name = a.Contains(FieldSeparator, StringComparison.OrdinalIgnoreCase) ? a.Split(FieldSeparator)[1] : a,
                          }).ToList();

            return result;
        }
    }
}
