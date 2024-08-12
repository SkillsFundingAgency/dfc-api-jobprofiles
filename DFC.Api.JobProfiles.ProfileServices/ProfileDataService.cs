using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Enums;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Common.SharedContent.Pkg.Netcore.Constant;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobProfileApiSkills = DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes.Skills;
using Skills = DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles.Skills;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public class ProfileDataService : IProfileDataService
    {
        private const double expiry = 24;
        private const string Published = "PUBLISHED";
        private readonly ILogger log;
        private readonly IMapper mapper;
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;

        public ProfileDataService(
            ILogger log,
            IMapper mapper,
            ISharedContentRedisInterface sharedContentRedisInterface)
        {
            this.log = log;
            this.mapper = mapper;
            this.sharedContentRedisInterface = sharedContentRedisInterface;
        }

        public async Task<JobProfileApiModel> GetJobProfile(string profileName)
        {
            var overview = await GetOverviewSegment(profileName, Published);

            if (overview == null)
            {
                return null;
            }

            var howToBecome = await GetHowToBecomeSegmentAsync(profileName, Published);
            var relatedCareers = await GetRelatedCareersSegmentAsync(profileName, Published);
            var careersPath = await GetCareerPathSegmentAsync(profileName, Published);
            var skills = await GetSkillSegmentAsync(profileName, Published);
            var tasks = await GetTasksSegmentAsync(profileName, Published);

            overview.HowToBecome = howToBecome;
            overview.RelatedCareers = relatedCareers;
            overview.CareerPathAndProgression = careersPath;
            overview.WhatYouWillDo = tasks;
            overview.WhatItTakes = skills;

            return overview;
        }

        public async Task<JobProfileApiModel> GetOverviewSegment(string canonicalName, string filter)
        {
            var overview = new JobProfileApiModel();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfilesOverviewResponse>(string.Concat(ApplicationKeys.JobProfileOverview, "/", canonicalName), filter, expiry);
                if (response.JobProfileOverview.Count == 0)
                {
                    return null;
                }

                overview = mapper.Map<JobProfileApiModel>(response);
                var overviewObject = JsonConvert.SerializeObject(overview, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });
                var result = JsonConvert.DeserializeObject<JobProfileApiModel>(overviewObject ?? string.Empty) ?? new JobProfileApiModel();
                return result;
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
            }

            return overview;
        }

        public async Task<HowToBecomeApiModel> GetHowToBecomeSegmentAsync(string canonicalName, string filter)
        {
            var howToBecome = new HowToBecomeApiModel();

            try
            {
                //Get the response from GraphQl
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileHowToBecomeResponse>(ApplicationKeys.JobProfileHowToBecome + "/" + canonicalName, filter, expiry);

                if (response.JobProfileHowToBecome != null)
                {
                    // Map CommonRoutes for College
                    var collegeCommonRoutes = mapper.Map<CommonRouteApiModel>(response, opt => opt.Items["RouteName"] = RouteName.College);

                    // Map CommonRoutes for University
                    var universityCommonRoutes = mapper.Map<CommonRouteApiModel>(response, opt => opt.Items["RouteName"] = RouteName.University);

                    // Map CommonRoutes for Apprenticeship
                    var apprenticeshipCommonRoutes = mapper.Map<CommonRouteApiModel>(response, opt => opt.Items["RouteName"] = RouteName.Apprenticeship);

                    var mappedMoreInfo = mapper.Map<MoreInformationApiModel>(response);

                    howToBecome = mapper.Map<HowToBecomeApiModel>(response);

                    var mappedEntryRoutes = mapper.Map<EntryRoutesApiModel>(response);

                    if (mappedEntryRoutes != null)
                    {
                        mappedEntryRoutes.University = universityCommonRoutes;
                        mappedEntryRoutes.Apprenticeship = apprenticeshipCommonRoutes;
                        mappedEntryRoutes.College = collegeCommonRoutes;
                    }

                    howToBecome.EntryRoutes = mappedEntryRoutes;

                    howToBecome.MoreInformation = mappedMoreInfo;
                    var howToBecomeObject = JsonConvert.SerializeObject(howToBecome, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });
                    var result = JsonConvert.DeserializeObject<HowToBecomeApiModel>(howToBecomeObject ?? string.Empty) ?? new HowToBecomeApiModel();

                    return result;
                }
                else
                {
                    return howToBecome;
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw;
            }
        }

        public async Task<List<RelatedCareerApiModel>> GetRelatedCareersSegmentAsync(string canonicalName, string status)
        {
            var relatedCareers = new List<RelatedCareerApiModel>();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<RelatedCareersResponse>(ApplicationKeys.JobProfileRelatedCareersPrefix + "/" + canonicalName, status, expiry);

                if (response.JobProfileRelatedCareers != null)
                {
                    var relatedCareersItems = response.JobProfileRelatedCareers.SelectMany(c => c.RelatedCareerProfiles.ContentItems).ToList();
                    var viewModel = mapper.Map<List<RelatedCareerApiModel>>(relatedCareersItems);
                    relatedCareers = viewModel;
                    var relatedCareersObject = JsonConvert.SerializeObject(relatedCareers, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });
                    var result = JsonConvert.DeserializeObject<List<RelatedCareerApiModel>>(relatedCareersObject ?? string.Empty) ?? new List<RelatedCareerApiModel>();
                    return result;
                }

                return relatedCareers;
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                throw;
            }
        }

        public async Task<CareerPathAndProgressionApiModel> GetCareerPathSegmentAsync(string canonicalName, string status)
        {
            CareerPathAndProgressionApiModel careerPath = new();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileCareerPathAndProgressionResponse>(ApplicationKeys.JobProfileCareerPath + "/" + canonicalName, status, expiry);

                if (response.JobProileCareerPath != null)
                {
                    var mappedResponse = mapper.Map<CareerPathAndProgressionApiModel>(response);

                    var careerPathObject = JsonConvert.SerializeObject(mappedResponse, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });
                    var result = JsonConvert.DeserializeObject<CareerPathAndProgressionApiModel>(careerPathObject ?? string.Empty) ?? new CareerPathAndProgressionApiModel();
                    return result;
                }

                return careerPath;
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                throw;
            }
        }



        public async Task<WhatItTakesApiModel> GetSkillSegmentAsync(string canonicalName, string status)
        {
            WhatItTakesApiModel skills = new WhatItTakesApiModel();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileSkillsResponse>(ApplicationKeys.JobProfileSkillsSuffix + "/" + canonicalName, status, expiry);
                var skillsResponse = await sharedContentRedisInterface.GetDataAsyncWithExpiry<SkillsResponse>(ApplicationKeys.SkillsAll, status, expiry);

                if (response.JobProfileSkills != null && skillsResponse.Skill != null)
                {
                    SkillsResponse jobProfileSkillsResponse = new SkillsResponse();
                    List<Skills> jobProfileSkillsList = new List<Skills>();

                    List<RelatedSkill> filteredSkills = response.JobProfileSkills.SelectMany(d => d.Relatedskills.ContentItems).ToList();

                    foreach (var skill in skillsResponse.Skill)
                    {
                        if (skill.DisplayText != null && filteredSkills.Any(d => d.RelatedSkillDesc.Equals(skill.DisplayText)))
                        {
                            jobProfileSkillsList.Add(skill);
                        }
                    }

                    jobProfileSkillsResponse.Skill = jobProfileSkillsList;

                    skills = mapper.Map<WhatItTakesApiModel>(response);

                    List<JobProfileApiSkills> sortedSkills = new List<JobProfileApiSkills>();
                    List<RelatedSkillsApiModel> relatedSkillsApiModels = new List<RelatedSkillsApiModel>();
                    var mappedSkillsResponse = mapper.Map<List<OnetSkill>>(jobProfileSkillsResponse.Skill);
                    var mappedContextualSkills = mapper.Map<List<ContextualisedSkill>>(filteredSkills);

                    foreach (var skill in filteredSkills)
                    {
                        sortedSkills.Add(new JobProfileApiSkills
                        {
                            ContextualisedSkill = mappedContextualSkills.Single(s => s.Description == skill.RelatedSkillDesc),
                            OnetSkill = mappedSkillsResponse.Single(s => s.Title == skill.RelatedSkillDesc),
                        });
                    }

                    foreach (var ski in sortedSkills)
                    {
                        relatedSkillsApiModels.Add(new RelatedSkillsApiModel
                        {
                            Description = ski.OnetSkill.Description,
                            ONetAttributeType = ski.ContextualisedSkill.ONetAttributeType,
                            ONetElementId = ski.OnetSkill.ONetElementId,
                            ONetRank = ski.ContextualisedSkill.ONetRank.ToString(),
                        });
                    }

                    var mappedRestriction = mapper.Map<RestrictionsAndRequirementsApiModel>(response);

                    skills.Skills = relatedSkillsApiModels;
                    skills.RestrictionsAndRequirements = mappedRestriction;

                    var skillsObject = JsonConvert.SerializeObject(skills, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });
                    var result = JsonConvert.DeserializeObject<WhatItTakesApiModel>(skillsObject ?? string.Empty) ?? new WhatItTakesApiModel();

                    return result;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }

            return skills;
        }


        public async Task<WhatYouWillDoApiModel> GetTasksSegmentAsync(string canonicalName, string filter)
        {
            var tasks = new WhatYouWillDoApiModel();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileWhatYoullDoResponse>(ApplicationKeys.JobProfileWhatYoullDo + "/" + canonicalName, filter, expiry);
                if (response.JobProfileWhatYoullDo != null)
                {
                    var mappedResponse = mapper.Map<WhatYouWillDoApiModel>(response);
                    var mappedEnvironment = mapper.Map<WorkingEnvironmentApiModel>(response);
                    mappedResponse.WorkingEnvironment = mappedEnvironment;

                    var tasksObject = JsonConvert.SerializeObject(mappedResponse, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });
                    var result = JsonConvert.DeserializeObject<WhatYouWillDoApiModel>(tasksObject ?? string.Empty) ?? new WhatYouWillDoApiModel();

                    return result;
                }

            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw;
            }

            return tasks;
        }
    }
}