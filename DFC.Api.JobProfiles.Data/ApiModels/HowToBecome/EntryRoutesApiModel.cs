using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.HowToBecome
{
    public class EntryRoutesApiModel
    {
        public CommonRouteApiModel University { get; set; }

        public CommonRouteApiModel College { get; set; }

        public CommonRouteApiModel Apprenticeship { get; set; }

        [Example(Description = "You may be able to start as a junior developer and improve your skills and knowledge by completing further training and qualifications while you work.")]
        public List<string> Work { get; set; }

        [Example(Description = "Volunteering information")]
        public List<string> Volunteering { get; set; }

        [Example(Description = "Direct application information")]
        public List<string> DirectApplication { get; set; }

        [Example(Description = "Other routes information")]
        public List<string> OtherRoutes { get; set; }
    }
}