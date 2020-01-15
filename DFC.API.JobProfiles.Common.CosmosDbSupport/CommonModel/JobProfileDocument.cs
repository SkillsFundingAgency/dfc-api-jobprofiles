using System;
using System.Collections.Generic;

namespace DFC.API.JobProfiles.Common.CosmosDbSupport.CommonModel
{
        public class MetaTags
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Keywords { get; set; }
        }

        public class Markup
        {
            public string CareerPath { get; set; }
            public string CurrentOpportunities { get; set; }
            public string HowToBecome { get; set; }
            public string OverviewBanner { get; set; }
            public string RelatedCareers { get; set; }
            public string WhatItTakes { get; set; }
            public string WhatYouWillDo { get; set; }
        }

        public class CareerPath
        {
            public DateTime LastReviewed { get; set; }
            public string Markup { get; set; }
        }

        public class Data
        {
            public CareerPath CareerPath { get; set; }
            public object CurrentOpportunities { get; set; }
            public object HowToBecome { get; set; }
            public object OverviewBanner { get; set; }
            public object RelatedCareers { get; set; }
            public object WhatItTakes { get; set; }
            public object WhatYouWillDo { get; set; }
        }

    public class JobProfileDocument
    {
        public string id { get; set; }
        public string _etag { get; set; }
        public string CanonicalName { get; set; }
        public string SocLevelTwo { get; set; }
        public string PartitionKey { get; set; }
        public DateTime LastReviewed { get; set; }
        public string BreadcrumbTitle { get; set; }
        public bool IncludeInSitemap { get; set; }
        public List<string> AlternativeNames { get; set; }
        public MetaTags MetaTags { get; set; }
        public Markup Markup { get; set; }
        public Data Data { get; set; }
        public string _rid { get; set; }
        public string _self { get; set; }
        public string _attachments { get; set; }
        public int _ts { get; set; }
    }
}
