using System;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.IntegrationTests.Model
{
    public class WorkingPattern
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class WorkingHoursDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class RouteEntry
    {
        public int RouteName { get; set; }
        public List<object> EntryRequirements { get; set; }
        public List<object> MoreInformationLinks { get; set; }
        public string RouteSubjects { get; set; }
        public string FurtherRouteInformation { get; set; }
        public object RouteRequirement { get; set; }
    }

    public class FurtherInformationModel
    {
        public string CareerTips { get; set; }
        public string ProfessionalAndIndustryBodies { get; set; }
        public string FurtherInformation { get; set; }
    }

    public class FurtherRoutes
    {
        public string Work { get; set; }
        public string Volunteering { get; set; }
        public string DirectApplication { get; set; }
        public string OtherRoutes { get; set; }
    }

    public class HowToBecomeData
    {
        public List<RouteEntry> RouteEntries { get; set; }
        public FurtherInformationModel FurtherInformation { get; set; }
        public FurtherRoutes FurtherRoutes { get; set; }
        public string IntroText { get; set; }
        public List<object> Registrations { get; set; }
    }

    public class WhatYouWillDoData
    {
        public string DailyTasks { get; set; }
        public List<object> Locations { get; set; }
        public List<object> Uniforms { get; set; }
        public List<object> Environments { get; set; }
        public string Introduction { get; set; }
    }

    public class ApprenticeshipFramework
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class ApprenticeshipStandard
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class SocCodeData
    {
        public string Id { get; set; }
        public string SOCCode { get; set; }
        public string Description { get; set; }
        public string ONetOccupationalCode { get; set; }
        public string UrlName { get; set; }
        public List<ApprenticeshipFramework> ApprenticeshipFramework { get; set; }
        public List<ApprenticeshipStandard> ApprenticeshipStandards { get; set; }
    }

    public class JobProfileCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    public class JobProfileCreateMessageBody
    {
        public string JobProfileId { get; set; }
        public string Title { get; set; }
        public string DynamicTitlePrefix { get; set; }
        public string AlternativeTitle { get; set; }
        public string Overview { get; set; }
        public string SocLevelTwo { get; set; }
        public string UrlName { get; set; }
        public string DigitalSkillsLevel { get; set; }
        public List<object> Restrictions { get; set; }
        public string OtherRequirements { get; set; }
        public string CareerPathAndProgression { get; set; }
        public string CourseKeywords { get; set; }
        public double MinimumHours { get; set; }
        public double MaximumHours { get; set; }
        public double SalaryStarter { get; set; }
        public double SalaryExperienced { get; set; }
        public List<WorkingPattern> WorkingPattern { get; set; }
        public List<object> WorkingPatternDetails { get; set; }
        public List<WorkingHoursDetail> WorkingHoursDetails { get; set; }
        public List<object> HiddenAlternativeTitle { get; set; }
        public List<object> JobProfileSpecialism { get; set; }
        public bool IsImported { get; set; }
        public HowToBecomeData HowToBecomeData { get; set; }
        public WhatYouWillDoData WhatYouWillDoData { get; set; }
        public SocCodeData SocCodeData { get; set; }
        public List<object> RelatedCareersData { get; set; }
        public List<object> SocSkillsMatrixData { get; set; }
        public List<JobProfileCategory> JobProfileCategories { get; set; }
        public DateTime LastModified { get; set; }
        public string CanonicalName { get; set; }
        public string WidgetContentTitle { get; set; }
        public bool IncludeInSitemap { get; set; }
    }
}
