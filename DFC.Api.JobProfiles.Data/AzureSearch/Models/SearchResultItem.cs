namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class SearchResultItem<T> : IDigitalDataModel
        where T : class
    {
        public double Rank { get; set; }

        public T ResultItem { get; set; }

        public double Score { get; set; }
    }
}