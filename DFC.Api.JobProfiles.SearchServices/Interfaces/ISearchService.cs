using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface ISearchService
    {
        Task<SearchApiModel> GetResultsList(string requestUrl, string searchTerm, int page, int pageSize);
    }
}