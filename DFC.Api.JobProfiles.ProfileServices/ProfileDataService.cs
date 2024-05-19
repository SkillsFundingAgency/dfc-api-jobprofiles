using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers;
using DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Data.Enums;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Common.SharedContent.Pkg.Netcore.Constant;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.Api.JobProfiles;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobProfileApiSkills = DFC.Api.JobProfiles.Data.ApiModels.WhatItTakes.Skills;
using Skills = DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles.Skills;
using DFC.Api.JobProfiles.AutoMapperProfile;
using DFC.Api.JobProfiles.AutoMapperProfile.Enums;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;

namespace DFC.Api.JobProfiles.ProfileServices
{
    public class ProfileDataService : IProfileDataService
    {
        private const string Published = "PUBLISHED";

        private readonly ICosmosRepository<SegmentDataModel> repository;
        private readonly ILogger log;

        private readonly IMapper mapper;
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;

        public ProfileDataService(
            ICosmosRepository<SegmentDataModel> repository,
            ILogger log,
            IMapper mapper,
            ISharedContentRedisInterface sharedContentRedisInterface)
        {
            this.repository = repository;
            this.log = log;
            this.mapper = mapper;
            this.sharedContentRedisInterface = sharedContentRedisInterface;
        }

        public async Task<JobProfileApiModel> GetJobProfile(string profileName)
        {
            var segment = new SegmentDataModel();
            var overview = await GetOverviewSegment(profileName, Published);
            var howToBecome = await GetHowToBecomeSegmentAsync(profileName, Published);
            var relatedCareers = await GetRelatedCareersSegmentAsync(profileName, Published);
            var careersPath = await GetCareerPathSegmentAsync(profileName, Published);
            var skills = await GetSkillSegmentAsync(profileName, Published);
            var tasks = await GetTasksSegmentAsync(profileName, Published);

            //segment.Segments.Add(related);

            var result = JsonConvert.DeserializeObject<JobProfileApiModel>(segment.)

            overview.HowToBecome = howToBecome;
            overview.RelatedCareers = relatedCareers;
            overview.CareerPathAndProgression = careersPath;
            overview.WhatYouWillDo = tasks;
            overview.WhatItTakes = skills;



            /*var segmentDetailModels = await repository.GetData(
                    s => new SegmentDataModel { Segments = s.Segments, CanonicalName = s.CanonicalName },
                    model => model.CanonicalName == profileName.ToLowerInvariant())
                 .ConfigureAwait(false);*/

           /* if (segmentDetailModels is null || !segmentDetailModels.Any())
            {
                return null;
            }

            var overviewDataModel = segmentDetailModels.FirstOrDefault()?.Segments?.FirstOrDefault(s => s.Segment == JobProfileSegment.Overview);
            var result = JsonConvert.DeserializeObject<JobProfileApiModel>(overviewDataModel?.Json ?? string.Empty) ?? new JobProfileApiModel();

            var otherSegmentDataModels = segmentDetailModels.FirstOrDefault()?.Segments?.Where(s => s.Segment != JobProfileSegment.Overview).ToList();
            if (otherSegmentDataModels == null || !otherSegmentDataModels.Any())
            {
                return result;
            }

            foreach (var segmentDetailModel in otherSegmentDataModels)
            {
                switch (segmentDetailModel.Segment)
                {
                    //case JobProfileSegment.HowToBecome:
                    //    {
                    //        var howToBecomeApiModel = JsonConvert.DeserializeObject<HowToBecomeApiModel>(howToBecome);
                    //        result.HowToBecome = howToBecomeApiModel;
                    //        break;
                    //    }

                    //case JobProfileSegment.CareerPathsAndProgression:
                    //    {
                    //        var careerPathAndProgressionApiModel = JsonConvert.DeserializeObject<CareerPathAndProgressionApiModel>(howToBecome);
                    //        result.CareerPathAndProgression = careerPathAndProgressionApiModel;
                    //        break;
                    //    }

                    //case JobProfileSegment.RelatedCareers:
                    //    {
                    //        var relatedCareerApiModels = JsonConvert.DeserializeObject<List<RelatedCareerApiModel>>(segmentDetailModel.Json);
                    //        result.RelatedCareers = relatedCareerApiModels;
                    //        break;
                    //    }

                    //case JobProfileSegment.WhatItTakes:
                    //    {
                    //        var whatItTakesApiModel = JsonConvert.DeserializeObject<WhatItTakesApiModel>(segmentDetailModel.Json);
                    //        result.WhatItTakes = whatItTakesApiModel;
                    //        break;
                    //    }

                    //case JobProfileSegment.WhatYouWillDo:
                    //    {
                    //        var whatYouWillDoApiModel = JsonConvert.DeserializeObject<WhatYouWillDoApiModel>(segmentDetailModel.Json);
                    //        result.WhatYouWillDo = whatYouWillDoApiModel;
                    //        break;
                    //    }

                    default:
                        {
                            log.LogWarning($"Unknown Segment Name ('{segmentDetailModel.Segment}') retrieved for Job Profile with name {profileName}");
                            break;
                        }
                }
            }*/

            return overview;
        }

        public async Task<bool> PingAsync()
        {
            return await repository.PingAsync().ConfigureAwait(false);
        }

        private async Task<JobProfileApiModel> GetOverviewSegment(string canonicalName, string filter)
        {
            var overview = new JobProfileApiModel();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfilesOverviewResponse>(string.Concat(ApplicationKeys.JobProfileOverview, "/", canonicalName), filter);
                overview = mapper.Map<JobProfileApiModel>(response);
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
            }

            return overview;
        }

        //private async Task<SegmentModel> GetHowToBecomeSegmentAsync(string canonicalName, string filter)
        private async Task<HowToBecomeApiModel> GetHowToBecomeSegmentAsync(string canonicalName, string filter)
        {
            //var howToBecome = new SegmentModel();
            var howToBecome = new HowToBecomeApiModel();

            try
            {
                //Get the response from GraphQl
               var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileHowToBecomeResponse>(ApplicationKeys.JobProfileHowToBecome + "/" + canonicalName, filter);

                //howToBecome = mapper.Map<HowToBecomeApiModel>(response);

                // Map the response to a HowToBecomeSegmentDataModel
                //var mappedResponse = mapper.Map<HowToBecomeSegmentDataModel>(response);

                // Map CommonRoutes for College
                var collegeCommonRoutes = mapper.Map<CommonRouteApiModel>(response, opt => opt.Items["RouteName"] = RouteName.College);

                // Map CommonRoutes for University
                var universityCommonRoutes = mapper.Map<CommonRouteApiModel>(response, opt => opt.Items["RouteName"] = RouteName.University);

                // Map CommonRoutes for Apprenticeship
                var apprenticeshipCommonRoutes = mapper.Map<CommonRouteApiModel>(response, opt => opt.Items["RouteName"] = RouteName.Apprenticeship);

                var mappedMoreInfo = mapper.Map<MoreInformationApiModel>(response);

                howToBecome.EntryRouteSummary = new List<string> { response.JobProfileHowToBecome.FirstOrDefault().EntryRoutes.Html };

                howToBecome.EntryRoutes = new EntryRoutesApiModel()
                {
                    University = universityCommonRoutes,
                    Apprenticeship = apprenticeshipCommonRoutes,
                    College = collegeCommonRoutes,
                    DirectApplication = new List<string>() { response.JobProfileHowToBecome.FirstOrDefault().DirectApplication.Html },
                    OtherRoutes = new List<string>() { response.JobProfileHowToBecome.FirstOrDefault().OtherRoutes.Html },
                    Volunteering = new List<string>() { response.JobProfileHowToBecome.FirstOrDefault().Volunteering.Html },
                    Work = new List<string>() { response.JobProfileHowToBecome.FirstOrDefault().Work.Html },
                };

               mappedMoreInfo.CareerTips = new List<string>() { response.JobProfileHowToBecome.FirstOrDefault().CareerTips.Html };
               mappedMoreInfo.FurtherInformation = new List<string>() { response.JobProfileHowToBecome.FirstOrDefault().FurtherInformation.Html };
               mappedMoreInfo.ProfessionalAndIndustryBodies = new List<string>() { response.JobProfileHowToBecome.FirstOrDefault().ProfessionalAndIndustryBodies.Html };

                howToBecome.MoreInformation = mappedMoreInfo;

                return howToBecome;
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw;
            }
        }

        private async Task<List<RelatedCareerApiModel>> GetRelatedCareersSegmentAsync(string canonicalName, string status)
        {
            var relatedCareers = new List<RelatedCareerApiModel>();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<RelatedCareersResponse>(ApplicationKeys.JobProfileRelatedCareersPrefix + "/" + canonicalName, status);

                if (response.JobProfileRelatedCareers != null)
                {
                    var relatedCareersItems = response.JobProfileRelatedCareers.SelectMany(c => c.RelatedCareerProfiles.ContentItems).ToList();
                    var viewModel = mapper.Map<List<RelatedCareerApiModel>>(relatedCareersItems);
                    relatedCareers = viewModel;
                }

                return relatedCareers;
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                throw;
            }
        }

        private async Task<CareerPathAndProgressionApiModel> GetCareerPathSegmentAsync(string canonicalName, string status)
        {
            //var careersPath = await GetCareerPathSegmentAsync(canonicalName, status);
            CareerPathAndProgressionApiModel careerPath = new();

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileCareerPathAndProgressionResponse>(ApplicationKeys.JobProfileCareerPath + "/" + canonicalName, status);

                if (response.JobProileCareerPath != null)
                {
                    var mappedResponse = mapper.Map<CareerPathAndProgressionApiModel>(response);
                }

                return careerPath;
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                throw;
            }
        }



        private async Task<WhatItTakesApiModel> GetSkillSegmentAsync(string canonicalName, string status)
        {
            WhatItTakesApiModel skills = new WhatItTakesApiModel();
            RestrictionsAndRequirementsApiModel restrictionsAndRequirements = new RestrictionsAndRequirementsApiModel()
            {
                OtherRequirements = new List<string>(),
                RelatedRestrictions = new List<string>(),
            };

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileSkillsResponse>(ApplicationKeys.JobProfileSkillsSuffix + "/" + canonicalName, status);
                var skillsResponse = await sharedContentRedisInterface.GetDataAsyncWithExpiry<SkillsResponse>(ApplicationKeys.SkillsAll, "PUBLISHED");

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

                    restrictionsAndRequirements.OtherRequirements.Add(response.JobProfileSkills.FirstOrDefault().Otherrequirements.Html);

                    var restrictions = response.JobProfileSkills.SelectMany(d => d.Relatedrestrictions.ContentItems).ToList();
                    foreach (var res in restrictions)
                    {
                        restrictionsAndRequirements.RelatedRestrictions.Add(res.Info.Html);
                    }

                    skills.Skills = relatedSkillsApiModels;
                    skills.RestrictionsAndRequirements = restrictionsAndRequirements;

                    return skills;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }

            return skills;
        }


        private async Task<WhatYouWillDoApiModel> GetTasksSegmentAsync(string canonicalName, string filter)
        {
            var tasks = new WhatYouWillDoApiModel()
            {
                WYDDayToDayTasks = new List<string>(),
            };

            try
            {
                var response = await sharedContentRedisInterface.GetDataAsyncWithExpiry<JobProfileWhatYoullDoResponse>(ApplicationKeys.JobProfileWhatYoullDo + "/" + canonicalName, filter);
                string test = Regex.Replace(response.JobProfileWhatYoullDo.FirstOrDefault().Daytodaytasks.Html, @"<[^>]+>| ", " ").Trim();
                if (response.JobProfileWhatYoullDo != null)
                {
                    var mappedResponse = mapper.Map<WorkingEnvironmentApiModel>(response);
                    tasks.WorkingEnvironment = mappedResponse;
                    tasks.WYDDayToDayTasks.Add(test);

                    return tasks;
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