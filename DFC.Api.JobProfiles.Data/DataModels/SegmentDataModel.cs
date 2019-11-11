using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.DataModels
{
    public class SegmentDataModel : BaseDataModel
    {
        public IList<SegmentDetailModel> Segments { get; set; }
    }
}