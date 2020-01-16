using System;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.IntegrationTests.Model
{
    public class JobProfileDetailsAPIResponse
    {
        public string Title { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Url { get; set; }
        public string Soc { get; set; }
        public string ONetOccupationalCode { get; set; }
        public string AlternativeTitle { get; set; }
        public string Overview { get; set; }
        public string SalaryStarter { get; set; }
        public string SalaryExperienced { get; set; }
        public double MinimumHours { get; set; }
        public double MaximumHours { get; set; }
        public string WorkingHoursDetails { get; set; }
        public string WorkingPattern { get; set; }
        public string WorkingPatternDetails { get; set; }
        public HowToBecomeModel HowToBecome { get; set; }
        public WhatItTakesModel WhatItTakes { get; set; }
        public WhatYouWillDoModel WhatYouWillDo { get; set; }
        public CareerPathAndProgressionApiModel CareerPathAndProgression { get; set; }
        public List<RelatedCareer> RelatedCareers { get; set; }

        public class University
        {
            public List<string> RelevantSubjects { get; set; }
            public List<string> FurtherInformation { get; set; }
            public string EntryRequirementPreface { get; set; }
            public List<string> EntryRequirements { get; set; }
            public List<string> AdditionalInformation { get; set; }
        }

        public class CareerPathAndProgressionApiModel
        {
            public List<string> CareerPathAndProgression { get; set; }
        }

        public class WhatYouWillDoModel
        {
            public List<string> WYDDayToDayTasks { get; set; }
            public WorkingEnvironment WorkingEnvironment { get; set; }
        }

        public class WhatItTakesModel
        {
            public string DigitalSkillsLevel { get; set; }
            public List<Skill> Skills { get; set; }
            public RestrictionsAndRequirements RestrictionsAndRequirements { get; set; }
        }

        public class HowToBecomeModel
        {
            public List<string> EntryRouteSummary { get; set; }
            public EntryRoutes EntryRoutes { get; set; }
            public MoreInformation MoreInformation { get; set; }
        }

        public class College
        {
            public List<object> RelevantSubjects { get; set; }
            public List<object> FurtherInformation { get; set; }
            public object EntryRequirementPreface { get; set; }
            public List<object> EntryRequirements { get; set; }
            public List<object> AdditionalInformation { get; set; }
        }

        public class Apprenticeship
        {
            public List<string> RelevantSubjects { get; set; }
            public List<string> FurtherInformation { get; set; }
            public string EntryRequirementPreface { get; set; }
            public List<string> EntryRequirements { get; set; }
            public List<string> AdditionalInformation { get; set; }
        }

        public class EntryRoutes
        {
            public University University { get; set; }
            public College College { get; set; }
            public Apprenticeship Apprenticeship { get; set; }
            public List<object> Work { get; set; }
            public List<string> Volunteering { get; set; }
            public List<object> DirectApplication { get; set; }
            public List<object> OtherRoutes { get; set; }
        }

        public class MoreInformation
        {
            public List<string> Registrations { get; set; }
            public List<object> CareerTips { get; set; }
            public List<object> ProfessionalAndIndustryBodies { get; set; }
            public List<string> FurtherInformation { get; set; }
        }

        public class Skill
        {
            public string Description { get; set; }
            public string ONetAttributeType { get; set; }
            public string ONetRank { get; set; }
            public string ONetElementId { get; set; }
        }

        public class RestrictionsAndRequirements
        {
            public List<string> RelatedRestrictions { get; set; }
            public List<object> OtherRequirements { get; set; }
        }

        public class WorkingEnvironment
        {
            public string Location { get; set; }
            public string Environment { get; set; }
            public string Uniform { get; set; }
        }

        public class RelatedCareer
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
    }
}
