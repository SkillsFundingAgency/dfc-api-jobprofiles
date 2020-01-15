using Microsoft.Azure.Search;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface ISharedConfigFactory
    {
        Task<ISearchIndexClient> GetSearchIndexClient();

        Task<ISearchIndexClient> CreateOrRefresh();
    }
}