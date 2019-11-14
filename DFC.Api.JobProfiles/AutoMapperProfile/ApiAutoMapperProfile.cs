using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Overview;
using DFC.Api.JobProfiles.Data.DataModels;
using System.Diagnostics.CodeAnalysis;

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

            CreateMap<OverviewApiModel, JobProfileApiModel>();
        }
    }
}