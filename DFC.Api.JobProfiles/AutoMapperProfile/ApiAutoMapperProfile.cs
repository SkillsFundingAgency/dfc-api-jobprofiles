using AutoMapper;
using DFC.Api.JobProfiles.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels;

namespace DFC.Api.JobProfiles.AutoMapperProfile
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