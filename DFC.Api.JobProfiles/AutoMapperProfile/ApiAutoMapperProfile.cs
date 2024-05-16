using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.ValueConverters;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.JSON.Standard.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

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

            CreateMap<JobProfilesOverviewResponse, SummaryApiModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.JobProfileOverview.FirstOrDefault().DisplayText))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.JobProfileOverview.FirstOrDefault().PageLocation.UrlName))
                .ForMember(d => d.LastUpdated, d => d.Ignore());

            CreateMap<JobProfileOverview, SummaryApiModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.DisplayText))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.PageLocation.UrlName))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => DateTime.UtcNow));

            CreateMap<JobProfileSummary, SummaryApiModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.DisplayText))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.PageLocation.UrlName))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => DateTime.Parse(s.PublishedUtc)));

           /* CreateMap<JArray, SummaryApiModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s["Title"]))
                .ForMember(d => d.Url, o => o.MapFrom(s => s["Url"]))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => s["LastUpdated"]));*/

          /*  CreateMap<JobProfileOverview, SummaryDataModel>()
                .ForMember(d => d.BreadcrumbTitle, o => o.MapFrom(s => s.DisplayText))
                .ForMember(d => d.CanonicalName, o => o.MapFrom(s => s.PageLocation.UrlName))
                .ForMember(d => d.LastReviewed, d => d.Ignore());*/
        }
    }
}