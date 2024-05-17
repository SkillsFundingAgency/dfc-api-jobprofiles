using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression
{
    public class CareerPathAndProgressionApiModel
    {
        [Example(Description = "With experience, you could specialise in a particular area, like e-commerce or move up to a more senior role like lead programmer or project leader.")]
        public string CareerPathAndProgression { get; set; }
    }
}