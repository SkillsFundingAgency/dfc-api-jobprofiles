using DFC.Swagger.Standard.Annotations;

namespace DFC.Api.JobProfiles.Data.ApiModels.Search
{
    public class JobProfileCategoryApiModel
    {
        [Example(Description = "Healthcare")]
        public string Title { get; set; }

        [Example(Description = "healthcare")]
        public string Name { get; set; }
    }
}
