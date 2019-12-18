using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AddWeightingAttribute : Attribute
    {
        public AddWeightingAttribute(double weighting)
        {
            this.Weighting = weighting;
        }

        public double Weighting { get; private set; }
    }
}
