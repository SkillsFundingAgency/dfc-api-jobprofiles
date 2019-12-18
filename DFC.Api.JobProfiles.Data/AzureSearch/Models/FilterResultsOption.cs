namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class FilterResultsOption
    {
        public string Id { get; set; }

        public bool IsSelected { get; set; }

        public string OptionKey { get; set; }

        public bool ClearOtherOptionsIfSelected { get; set; }
    }
}