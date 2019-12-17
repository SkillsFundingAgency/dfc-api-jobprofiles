using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices
{
    public class SearchService : ISearchService
    {
        private readonly IMapper mapper;
        private readonly ISearchQueryService<JobProfileIndex> searchQueryService;

        public SearchService(
                IMapper mapper,
                ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            this.mapper = mapper;
            this.searchQueryService = searchQueryService;
        }

        public async Task<IList<SearchApiModel>> GetResutsList(string requestUrl, string searchTerm, int page, int pageSize)
        {
            const bool useRawSearchTerm = true;
            var pageNumber = page > 0 ? page : 1;
            var searchProperties = new SearchProperties { Page = pageNumber, Count = pageSize, UseRawSearchTerm = useRawSearchTerm };

            var searchResult = await searchQueryService.SearchAsync(searchTerm, searchProperties).ConfigureAwait(false);

            if (searchResult?.Results is null)
            {
                return null;
            }

            var viewModels = mapper.Map<List<SearchApiModel>>(searchResult.Results);
            viewModels.ForEach(v => v.ResultItemUrlName = $"{requestUrl}{v.ResultItemUrlName.TrimStart('/')}");

            return viewModels;
        }
    }
}