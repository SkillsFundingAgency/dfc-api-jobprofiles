using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Functions.ApiModels.HowToBecome
{
    public class CommonRouteApiModel
    {
        public string EntryRequirementPreface { get; set; }

        public List<string> RelevantSubjects { get; set; }

        public List<string> FurtherInformation { get; set; }

        public List<string> EntryRequirements { get; set; }

        public List<string> AdditionalInformation { get; set; }
    }
}