using Azure.Search.Documents;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.AzureSearch;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.SearchServices
{
    public class DfcSearchQueryService<T> : AzSearchQueryService<T>, ISearchQueryService<T>
        where T : class
    {
        private readonly ISearchQueryBuilder queryBuilder;
        private readonly ISearchManipulator<T> searchManipulator;

        public DfcSearchQueryService(
            IAzSearchQueryConverter queryConverter,
            ISearchQueryBuilder queryBuilder,
            ISearchManipulator<T> searchManipulator,
            SearchClient searchClient)
            : base(queryConverter, searchClient)
        {
            this.queryBuilder = queryBuilder;
            this.searchManipulator = searchManipulator;
        }

        public override async Task<SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties = null)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var trimmedSearchTerm = queryBuilder.TrimCommonWordsAndSuffixes(cleanedSearchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(trimmedSearchTerm, properties);
            var finalComputedSearchTerm = searchManipulator.BuildSearchExpression(searchTerm, cleanedSearchTerm, partialTermToSearch, properties);

            var searchProperties = properties ?? new SearchProperties();
            var res = await base.SearchAsync(finalComputedSearchTerm, searchProperties).ConfigureAwait(false);
            var orderedResult = searchManipulator.Reorder(res, searchTerm, searchProperties);

            return orderedResult;
        }
    }
}