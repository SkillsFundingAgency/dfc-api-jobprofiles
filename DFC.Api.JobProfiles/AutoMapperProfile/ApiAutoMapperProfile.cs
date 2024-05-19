using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Resolvers;
using DFC.Api.JobProfiles.AutoMapperProfile.ValueConverters;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.JSON.Standard.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using JobProfSkills = DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles.Skills;
using RelatedSkill = DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles.RelatedSkill;
using Skills = DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes.Skills;

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

            CreateMap<JArray, SummaryApiModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s["Title"]))
                .ForMember(d => d.Url, o => o.MapFrom(s => s["Url"]))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => s["LastUpdated"]));

            CreateMap<JobProfilesOverviewResponse, JobProfileApiModel>()
            .ForMember(d => d.Title, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().DisplayText))
            .ForMember(d => d.LastUpdatedDate, option => option.Ignore())
            .ForMember(d => d.Url, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().PageLocation.UrlName))
            .ForMember(d => d.Soc, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().SocCode.ContentItems.FirstOrDefault().DisplayText))
            .ForMember(d => d.Soc2020, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().SocCode.ContentItems.FirstOrDefault().SOC2020))
            .ForMember(d => d.Soc2020Extension, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().SocCode.ContentItems.FirstOrDefault().SOC2020extension))
            .ForMember(d => d.ONetOccupationalCode, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().SocCode.ContentItems.FirstOrDefault().OnetOccupationCode))
            .ForMember(d => d.AlternativeTitle, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().AlternativeTitle))
            .ForMember(d => d.Overview, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().Overview))
            .ForMember(d => d.SalaryStarter, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().SalaryStarter))
            .ForMember(d => d.SalaryExperienced, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().SalaryExperienced))
            .ForMember(d => d.MinimumHours, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().Minimumhours))
            .ForMember(d => d.MaximumHours, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().Maximumhours))
            .ForMember(d => d.WorkingHoursDetails, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().WorkingHoursDetails.ContentItems.FirstOrDefault().DisplayText ?? string.Empty))
            .ForMember(d => d.WorkingPattern, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().WorkingPattern.ContentItems.FirstOrDefault().DisplayText ?? string.Empty))
            .ForMember(d => d.WorkingPatternDetails, s => s.MapFrom(a => a.JobProfileOverview.FirstOrDefault().WorkingPatternDetails.ContentItems.FirstOrDefault().DisplayText ?? string.Empty))
            .ForMember(d => d.HowToBecome, option => option.Ignore())
            .ForMember(d => d.WhatItTakes, option => option.Ignore())
            .ForMember(d => d.WhatYouWillDo, option => option.Ignore())
            .ForMember(d => d.CareerPathAndProgression, option => option.Ignore())
            .ForMember(d => d.LastUpdatedDate, option => option.Ignore())
            .ForMember(d => d.RelatedCareers, option => option.Ignore());

            CreateMap<RelatedCareersContentItems, RelatedCareerApiModel>()
                .ForMember(d => d.Title, s => s.MapFrom(a => a.DisplayText))
                .ForMember(d => d.Url, s => s.MapFrom(a => a.PageLocation.FullUrl));

            CreateMap<JobProfileCareerPathAndProgressionResponse, CareerPathAndProgressionApiModel>()
                .ForMember(d => d.CareerPathAndProgression, s => s.MapFrom(a => a.JobProileCareerPath.FirstOrDefault().Content.Html));

            CreateMap<JobProfileSkillsResponse, WhatItTakesApiModel>()
                .ForMember(d => d.DigitalSkillsLevel, s => s.MapFrom(a => a.JobProfileSkills.FirstOrDefault().DigitalSkills.ContentItems.FirstOrDefault().DisplayText))
                .ForMember(d => d.Skills, d => d.Ignore())
                .ForMember(d => d.RestrictionsAndRequirements, d => d.Ignore());

            CreateMap<JobProfileApiSkills, Skills>()
               .ForMember(d => d.OnetSkill, s => s.MapFrom(a => a.Skills))
               .ForMember(d => d.ContextualisedSkill, s => s.MapFrom(a => a.JobProfileContextualisedSkills));

            CreateMap<RelatedSkill, ContextualisedSkill>()
               .ForMember(d => d.ONetRank, s => s.MapFrom(a => a.ONetRank))
               .ForMember(d => d.ONetAttributeType, s => s.MapFrom(a => a.ONetAttributeType))
               .ForMember(d => d.Description, s => s.MapFrom(a => a.RelatedSkillDesc))
               .ForMember(d => d.Id, s => s.MapFrom(a => a.GraphSync.NodeId.Substring(a.GraphSync.NodeId.LastIndexOf('/') + 1)))
               .ForMember(d => d.OriginalRank, d => d.Ignore());

            CreateMap<JobProfSkills, OnetSkill>()
                .ForMember(d => d.ONetElementId, s => s.MapFrom(a => a.ONetElementId))
                .ForMember(d => d.Title, s => s.MapFrom(a => a.DisplayText))
                .ForMember(d => d.Description, s => s.MapFrom(a => a.Description))
                .ForMember(d => d.Id, s => s.MapFrom(a => a.GraphSync.NodeId.Substring(a.GraphSync.NodeId.LastIndexOf('/') + 1)));

            CreateMap<JobProfileApiSkills, RelatedSkillsApiModel>()
                .ForMember(d => d.Description, s => s.MapFrom(a => a.JobProfileContextualisedSkills.RelatedSkillDesc))
                .ForMember(d => d.ONetElementId, s => s.MapFrom(a => a.Skills.ONetElementId))
                .ForMember(d => d.ONetRank, s => s.MapFrom(a => a.JobProfileContextualisedSkills.ONetRank))
                .ForMember(d => d.ONetAttributeType, s => s.MapFrom(a => a.JobProfileContextualisedSkills.ONetAttributeType));

            CreateMap<JobProfileWhatYoullDoResponse, WorkingEnvironmentApiModel>()
                .ForMember(d => d.Location, s => s.MapFrom<LocationResolver>())
                .ForMember(d => d.Environment, s => s.MapFrom<EnvironmentResolver>())
                .ForMember(d => d.Uniform, s => s.MapFrom<UniformResolver>());

            CreateMap<JobProfileHowToBecomeResponse, MoreInformationApiModel>()
                .ForMember(d => d.Registrations, s => s.MapFrom<RegistrationResolver>())
                .ForMember(d => d.CareerTips, s => s.Ignore())
                .ForMember(d => d.ProfessionalAndIndustryBodies, s => s.Ignore())
                .ForMember(d => d.FurtherInformation, s => s.Ignore());

            CreateMap<JobProfileHowToBecomeResponse, CommonRouteApiModel>()
                .ForMember(d => d.EntryRequirementPreface, s => s.MapFrom<EntryRequirementsPrefaceResolver>())
                .ForMember(d => d.EntryRequirements, s => s.MapFrom<EntryRequirementsResolver>())
                .ForMember(d => d.FurtherInformation, s => s.MapFrom<FurtherRouteInfoResolver>())
                .ForMember(d => d.AdditionalInformation, s => s.MapFrom<AdditionalInfoResolver>())
                .ForMember(d => d.RelevantSubjects, s => s.MapFrom<RelevantSubjectsResolver>());

        }
    }
}