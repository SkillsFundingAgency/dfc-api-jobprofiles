using DFC.Api.JobProfiles.Data.Enums;

namespace DFC.Api.JobProfiles.Data.DataModels
{
    public class SegmentDetailModel
    {
        public JobProfileSegment Segment { get; set; }

        public string Json { get; set; }
    }
}