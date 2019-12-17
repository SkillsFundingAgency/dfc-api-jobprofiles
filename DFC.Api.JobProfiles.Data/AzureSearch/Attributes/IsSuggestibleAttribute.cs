using System;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IsSuggestibleAttribute : Attribute
    {
    }
}