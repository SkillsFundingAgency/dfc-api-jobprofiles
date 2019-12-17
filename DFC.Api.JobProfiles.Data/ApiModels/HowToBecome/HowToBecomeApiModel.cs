using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.HowToBecome
{
    public class HowToBecomeApiModel
    {
        [Example(Description = "You can get into this job through: a university course; a college course; an apprenticeship; working towards this role")]
        public List<string> EntryRouteSummary { get; set; }

        public EntryRoutesApiModel EntryRoutes { get; set; }

        public MoreInformationApiModel MoreInformation { get; set; }
    }
}