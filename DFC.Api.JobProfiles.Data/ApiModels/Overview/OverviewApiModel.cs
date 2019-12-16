using DFC.Swagger.Standard.Annotations;
using Newtonsoft.Json;
using System;

namespace DFC.Api.JobProfiles.Data.ApiModels.Overview
{
    public class OverviewApiModel
    {
        [JsonProperty(Order = -2)]
        [Example(Description = "Web developer")]
        public string Title { get; set; }

        [Example(Description = "2019-01-30T00:00:00.00Z")]
        public DateTime LastUpdatedDate { get; set; }

        [JsonProperty(Order = -1)]
        [Example(Description = "http://api-url/web-developer")]
        public string Url { get; set; }

        [Example(Description = "2137E")]
        public string Soc { get; set; }

        [Example(Description = "15-1134.00")]
        public string ONetOccupationalCode { get; set; }

        [Example(Description = "Web Dev")]
        public string AlternativeTitle { get; set; }

        [Example(Description = "Web developers create and maintain websites and web applications.")]
        public string Overview { get; set; }

        [Example(Description = "20000")]
        public string SalaryStarter { get; set; }

        [Example(Description = "50000")]
        public string SalaryExperienced { get; set; }

        [Example(Description = "37.0")]
        public decimal MinimumHours { get; set; }

        [Example(Description = "39.0")]
        public decimal MaximumHours { get; set; }

        [Example(Description = "a week")]
        public string WorkingHoursDetails { get; set; }

        [Example(Description = "evenings")]
        public string WorkingPattern { get; set; }

        [Example(Description = "occasionally")]
        public string WorkingPatternDetails { get; set; }
    }
}