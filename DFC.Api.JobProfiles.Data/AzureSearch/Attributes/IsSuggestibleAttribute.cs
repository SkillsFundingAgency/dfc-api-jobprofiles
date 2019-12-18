using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IsSuggestibleAttribute : Attribute
    {
    }
}