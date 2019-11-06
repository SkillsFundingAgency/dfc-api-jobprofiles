using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.Api.JobProfiles.Data.Models
{
    public class SummaryDataModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; set; }

        [Required]
        public string SocLevelTwo { get; set; }

        public string PartitionKey => SocLevelTwo;

        public string CanonicalName { get; set; }

        public string BreadcrumbTitle { get; set; }

        public List<string> AlternativeNames { get; set; }
    }
}