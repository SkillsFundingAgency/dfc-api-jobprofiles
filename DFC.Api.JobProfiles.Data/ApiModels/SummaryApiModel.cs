using DFC.Swagger.Standard.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace DFC.Api.JobProfiles.Data.ApiModels
{
    public class SummaryApiModel
    {
        [JsonProperty("Url")]
        [Example(Description = "http://api-url/web-developer")]
        public string Url { get; set; }

        [JsonProperty("Title")]
        [Example(Description = "Web Developer")]
        public string Title { get; set; }

        [JsonProperty("LastUpdated")]
        [Example(Description = "2019-01-30T00:00:00.00Z")]
        public DateTime LastUpdated { get; set; }
    }
}