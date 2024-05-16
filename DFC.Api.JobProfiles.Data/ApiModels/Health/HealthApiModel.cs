using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Data.ApiModels.Health
{
    [ExcludeFromCodeCoverage]
    public class HealthApiModel
    {
        public IList<HealthItemApiModel>? HealthItems { get; set; }
    }
}
