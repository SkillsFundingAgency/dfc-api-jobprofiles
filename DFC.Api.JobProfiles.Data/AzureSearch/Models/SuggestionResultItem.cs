namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class SuggestionResultItem<T>
        where T : class
    {
        public string MatchedSuggestion { get; set; }

        public T Index { get; set; }
    }
}