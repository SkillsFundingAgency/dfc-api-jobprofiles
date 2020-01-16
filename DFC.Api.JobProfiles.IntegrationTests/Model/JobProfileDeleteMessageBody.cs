namespace DFC.Api.JobProfiles.IntegrationTests.Model
{
    public class JobProfileDeleteMessageBody
    {
            public string JobProfileId { get; set; }
            public string Title { get; set; }
            public object DynamicTitlePrefix { get; set; }
            public object AlternativeTitle { get; set; }
            public object Overview { get; set; }
            public object SocLevelTwo { get; set; }
            public object UrlName { get; set; }
            public object DigitalSkillsLevel { get; set; }
            public object Restrictions { get; set; }
            public object OtherRequirements { get; set; }
            public object CareerPathAndProgression { get; set; }
            public object CourseKeywords { get; set; }
            public object MinimumHours { get; set; }
            public object MaximumHours { get; set; }
            public object SalaryStarter { get; set; }
            public object SalaryExperienced { get; set; }
            public object WorkingPattern { get; set; }
            public object WorkingPatternDetails { get; set; }
            public object WorkingHoursDetails { get; set; }
            public object HiddenAlternativeTitle { get; set; }
            public object JobProfileSpecialism { get; set; }
            public object IsImported { get; set; }
            public object HowToBecomeData { get; set; }
            public object WhatYouWillDoData { get; set; }
            public object SocCodeData { get; set; }
            public object RelatedCareersData { get; set; }
            public object SocSkillsMatrixData { get; set; }
            public object JobProfileCategories { get; set; }
            public string LastModified { get; set; }
            public object CanonicalName { get; set; }
            public object WidgetContentTitle { get; set; }
            public bool IncludeInSitemap { get; set; }
    }
}
