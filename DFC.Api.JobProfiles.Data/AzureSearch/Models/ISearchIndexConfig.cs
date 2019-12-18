namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public interface ISearchIndexConfig
    {
        string SearchIndex { get; }

        string AccessKey { get; }
    }
}