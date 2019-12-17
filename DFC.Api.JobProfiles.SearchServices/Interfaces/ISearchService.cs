using DFC.Api.JobProfiles.Data.ApiModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices.Interfaces
{
    public interface ISearchService
    {
        Task<IList<SearchApiModel>> GetResutsList(string requestUrl, string searchTerm, int page, int pageSize);
    }
}