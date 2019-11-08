using System;

namespace DFC.Api.JobProfiles.Data.DataModels
{
    public class SummaryDataModel
    {
        public string CanonicalName { get; set; }

        public string BreadcrumbTitle { get; set; }

        public DateTime LastReviewed { get; set; }
    }
}