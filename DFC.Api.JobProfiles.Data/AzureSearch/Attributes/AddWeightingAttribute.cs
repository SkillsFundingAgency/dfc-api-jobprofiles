using System;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Attributes
{
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
