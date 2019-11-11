using System;

namespace DFC.Api.JobProfiles.Data.DataModels
{
    public class SummaryDataModel : BaseDataModel
    {
        public string BreadcrumbTitle { get; set; }

        public DateTime LastReviewed { get; set; }
    }
}