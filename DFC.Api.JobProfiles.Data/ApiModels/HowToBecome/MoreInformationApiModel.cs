using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.HowToBecome
{
    public class MoreInformationApiModel
    {
        [Example(Description = "Registration information.")]
        public List<string> Registrations { get; set; }

        [Example(Description = "Make sure that you're up to date with the latest industry trends and web development standards.")]
        public List<string> CareerTips { get; set; }

        [Example(Description = "Professional body example")]
        public List<string> ProfessionalAndIndustryBodies { get; set; }

        [Example(Description = "You can get more advice about working in computing from [Tech Future Careers | https://www.tpdegrees.com/careers/] and\n[The Chartered Institute for IT. | https://www.bcs.org/category/5672]")]
        public List<string> FurtherInformation { get; set; }
    }
}