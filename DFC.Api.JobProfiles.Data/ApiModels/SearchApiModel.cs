//using DFC.Swagger.Standard.Annotations;
using System;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Data.ApiModels
{
    public class SearchApiModel
    {
        //        [Example(Description = "2")]
        public int Rank { get; set; }

        //        [Example(Description = "xxxxxxxxxxxxxxx")]
        public string ResultItemTitle { get; set; }

        //        [Example(Description = "xxxxxxxxxxxxxxx")]
        public string ResultItemAlternativeTitle { get; set; }

        //        [Example(Description = "xxxxxxxxxxxxxxx")]
        public string ResultItemOverview { get; set; }

        //        [Example(Description = "xxxxxxxxxxxxxxx")]
        public string ResultItemSalaryRange { get; set; }

        //        [Example(Description = "xxxxxxxxxxxxxxx")]
        public string ResultItemUrlName { get; set; }

        //        [Example(Description = "http://api-url/web-developer")]
        public IEnumerable<string> JobProfileCategoriesWithUrl { get; set; }

        //        [Example(Description = "3.2")]
        public double Score { get; set; }
    }
}