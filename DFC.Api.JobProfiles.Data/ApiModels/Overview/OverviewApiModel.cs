using Newtonsoft.Json;
using System;

namespace DFC.Api.JobProfiles.Data.ApiModels.Overview
{
    public class OverviewApiModel
    {
        [JsonProperty(Order = -2)]
        public string Title { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        [JsonProperty(Order = -1)]
        public string Url { get; set; }

        public string Soc { get; set; }

        public string ONetOccupationalCode { get; set; }

        public string AlternativeTitle { get; set; }

        public string Overview { get; set; }

        public string SalaryStarter { get; set; }

        public string SalaryExperienced { get; set; }

        public decimal MinimumHours { get; set; }

        public decimal MaximumHours { get; set; }

        public string WorkingHoursDetails { get; set; }

        public string WorkingPattern { get; set; }

        public string WorkingPatternDetails { get; set; }
    }
}