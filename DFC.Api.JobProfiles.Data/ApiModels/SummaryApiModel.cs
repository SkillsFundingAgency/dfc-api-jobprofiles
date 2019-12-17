using DFC.Swagger.Standard.Annotations;
using System;

namespace DFC.Api.JobProfiles.Data.ApiModels
{
    public class SummaryApiModel
    {
        [Example(Description = "http://api-url/web-developer")]
        public string Url { get; set; }

        [Example(Description = "Web Developer")]
        public string Title { get; set; }

        [Example(Description = "2019-01-30T00:00:00.00Z")]
        public DateTime LastUpdated { get; set; }
    }
}