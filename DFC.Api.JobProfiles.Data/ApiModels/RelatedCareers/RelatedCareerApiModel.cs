using DFC.Swagger.Standard.Annotations;

namespace DFC.Api.JobProfiles.Data.ApiModels.RelatedCareers
{
    public class RelatedCareerApiModel
    {
        [Example(Description = "Software developer")]
        public string Title { get; set; }

        [Example(Description = "http://api-url/software-developer")]
        public string Url { get; set; }
    }
}