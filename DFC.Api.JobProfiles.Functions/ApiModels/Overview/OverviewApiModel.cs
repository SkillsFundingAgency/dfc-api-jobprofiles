namespace DFC.Api.JobProfiles.Functions.ApiModels.Overview
{
    public class OverviewApiModel
    {
        public string Title { get; set; }

        public string LastUpdatedDate { get; set; }

        public string Url { get; set; }

        public string Soc { get; set; }

        public string ONetOccupationalCode { get; set; }

        public string AlternativeTitle { get; set; }

        public string Overview { get; set; }

        public decimal SalaryStarter { get; set; }

        public decimal SalaryExperienced { get; set; }

        public decimal MinimumHours { get; set; }

        public decimal MaximumHours { get; set; }

        public string WorkingHoursDetails { get; set; }

        public string WorkingPattern { get; set; }

        public string WorkingPatternDetails { get; set; }
    }
}