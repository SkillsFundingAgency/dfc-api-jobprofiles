using System;

namespace DFC.Api.JobProfiles.ApiModels
{
    public class SummaryApiModel
    {
        public string FullUrl { get; set; }

        public string Title { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}