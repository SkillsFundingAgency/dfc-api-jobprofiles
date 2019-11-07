using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Functions.ApiModels.HowToBecome
{
    public class HowToBecomeApiModel
    {
        public List<string> EntryRouteSummary { get; set; }

        public EntryRoutesApiModel EntryRoutes { get; set; }

        public MoreInformationApiModel MoreInformation { get; set; }
    }
}