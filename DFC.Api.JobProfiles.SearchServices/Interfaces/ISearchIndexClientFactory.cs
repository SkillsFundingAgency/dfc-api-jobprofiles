using Microsoft.Azure.Search;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface ISearchIndexClientFactory
    {
        Task<ISearchIndexClient> GetSearchIndexClient();

        Task<ISearchIndexClient> CreateOrRefreshIndexClient();
    }
}