using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.ValueConverters;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.DataModels;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DFC.Api.JobProfiles.AutoMapperProfile
{
    [ExcludeFromCodeCoverage]
    public class ApiAutoMapperProfile : Profile
    {
        public ApiAutoMapperProfile()
        {
            CreateMap<SummaryDataModel, SummaryApiModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.BreadcrumbTitle))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => s.LastReviewed))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.CanonicalName));

            CreateMap<SearchResult<JobProfileIndex>, SearchApiModel>()
                .ForMember(d => d.CurrentPage, o => o.Ignore())
                .ForMember(d => d.PageSize, o => o.Ignore());

            CreateMap<SearchResultItem<JobProfileIndex>, SearchItemApiModel>()
                .ForMember(d => d.ResultItemAlternativeTitle, o => o.MapFrom(s => string.Join(", ", s.ResultItem.AlternativeTitle).Trim().TrimEnd(',')))
                .ForMember(c => c.JobProfileCategories, opt => opt.ConvertUsing(new JobProfileCategoryConverter(), a => a.ResultItem.JobProfileCategoriesWithUrl))
                .ForMember(d => d.ResultItemSalaryRange, o => o.MapFrom(s => s.ResultItem.SalaryStarter.Equals(0) || s.ResultItem.SalaryExperienced.Equals(0) ? string.Empty : string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", s.ResultItem.SalaryStarter, s.ResultItem.SalaryExperienced)));
        }
    }
}