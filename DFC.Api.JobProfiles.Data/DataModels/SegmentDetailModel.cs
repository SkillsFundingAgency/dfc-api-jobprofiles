using DFC.Api.JobProfiles.Data.Enums;
using Newtonsoft.Json;

namespace DFC.Api.JobProfiles.Data.DataModels
{
    public class SegmentDetailModel
    {
        public JobProfileSegment Segment { get; set; }

        public string JsonV1 { get; set; }

        [JsonIgnore]
        public string Json { get => JsonV1; set => JsonV1 = value; }
    }
}