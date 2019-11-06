using System;
using System.Collections.Generic;

namespace DFC.Api.JobProfiles.Functions.ViewModels
{
    public class SummaryViewModel
    {
        public Guid JobProfileId { get; set; }

        public string CanonicalName { get; set; }

        public string Title { get; set; }

        public List<string> AlternativeNames { get; set; }
    }
}