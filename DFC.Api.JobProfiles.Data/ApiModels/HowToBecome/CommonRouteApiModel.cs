using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.HowToBecome
{
    public class CommonRouteApiModel
    {
        [Example(Description = "You could do a foundation degree, higher national diploma or degree in: web design and development; computer science; digital media development; software engineering")]
        public List<string> RelevantSubjects { get; set; }

        [Example(Description = "Some further information")]
        public List<string> FurtherInformation { get; set; }

        [Example(Description = "You'll usually need:")]
        public string EntryRequirementPreface { get; set; }

        [Example(Description = "1 or 2 A levels for a foundation degree or higher national diploma, 2 to 3 A levels for a degree")]
        public List<string> EntryRequirements { get; set; }

        [Example(Description = "equivalent entry requirements | https://www.gov.uk/what-different-qualification-levels-mean/list-of-qualification-levels, student finance for fees and living costs | https://www.gov.uk/student-finance], [university courses and entry requirements | https://www.ucas.com/")]
        public List<string> AdditionalInformation { get; set; }
    }
}