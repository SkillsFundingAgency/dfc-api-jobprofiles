using AutoMapper;
using DFC.Api.JobProfiles.Data.Models;
using DFC.Api.JobProfiles.Functions.ApiModels;

namespace DFC.Api.JobProfiles.Functions.AutoMapperProfile
{
    public class ApiAutoMapperProfile : Profile
    {
        public ApiAutoMapperProfile()
        {
            CreateMap<SummaryDataModel, SummaryApiModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.BreadcrumbTitle))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => s.LastReviewed))
                .ForMember(d => d.FullUrl, o => o.MapFrom(s => s.CanonicalName));
        }
    }
}