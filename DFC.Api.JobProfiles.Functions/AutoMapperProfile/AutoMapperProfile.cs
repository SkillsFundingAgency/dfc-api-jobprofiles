using AutoMapper;
using DFC.Api.JobProfiles.Data.Models;
using DFC.Api.JobProfiles.Functions.ViewModels;

namespace DFC.Api.JobProfiles.Functions.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SummaryDataModel, SummaryViewModel>()
                .ForMember(d => d.JobProfileId, o => o.MapFrom(s => s.DocumentId))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.BreadcrumbTitle));
        }
    }
}