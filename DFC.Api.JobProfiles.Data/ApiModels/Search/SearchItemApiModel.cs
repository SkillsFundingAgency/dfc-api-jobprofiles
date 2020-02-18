using DFC.Swagger.Standard.Annotations;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels.Search
{
    public class SearchItemApiModel
    {
        [Example(Description = "Nurse")]
        public string ResultItemTitle { get; set; }

        [Example(Description = "Adult nurse")]
        public string ResultItemAlternativeTitle { get; set; }

        [Example(Description = "Nurses care for adults who are sick, injured or have physical disabilities.")]
        public string ResultItemOverview { get; set; }

        [Example(Description = "£24,214 to £37,267")]
        public string ResultItemSalaryRange { get; set; }

        [Example(Description = "http://api-url/nurse")]
        public string ResultItemUrlName { get; set; }

        [Example(Description = "List of job categories")]
        public IEnumerable<JobProfileCategoryApiModel> JobProfileCategories { get; set; }
    }
}