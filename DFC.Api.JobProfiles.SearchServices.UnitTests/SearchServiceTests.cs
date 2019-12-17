using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests
{
    public class SearchServiceTests
    {
        private readonly IMapper mapper;
        private readonly ISearchQueryService<JobProfileIndex> searchQueryService;

        public SearchServiceTests()
        {
            mapper = A.Fake<IMapper>();
            searchQueryService = A.Fake<ISearchQueryService<JobProfileIndex>>();
        }

        [Fact]
        public async Task GetResutsListReturnsSuccessForHappyPath()
        {
            // Arrange
            const string requestUrl = "http://Something.com/";
            const string searchTerm = "nurse";
            const int page = 1;
            const int pageSize = 10;
            var searchResults = new SearchResult<JobProfileIndex>()
            {
                Results = A.CollectionOfFake<SearchResultItem<JobProfileIndex>>(2),
            };
            var expectedResults = A.CollectionOfFake<SearchApiModel>(2).ToList();
            var summaryService = new SearchService(mapper, searchQueryService);

            A.CallTo(() => searchQueryService.SearchAsync(A<string>.Ignored, A<SearchProperties>.Ignored)).Returns(searchResults);
            A.CallTo(() => mapper.Map<List<SearchApiModel>>(searchResults.Results)).Returns(expectedResults);

            // Act
            var results = await summaryService.GetResutsList(requestUrl, searchTerm, page, pageSize).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchQueryService.SearchAsync(A<string>.Ignored, A<SearchProperties>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mapper.Map<List<SearchApiModel>>(searchResults.Results)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResults.Count, results.Count);
        }

        [Fact]
        public async Task GetResutsListReturnsNullForNoResults()
        {
            // Arrange
            const string requestUrl = "http://Something.com/";
            const string searchTerm = "nurse";
            const int page = 1;
            const int pageSize = 10;
            var searchResults = new SearchResult<JobProfileIndex>();
            var summaryService = new SearchService(mapper, searchQueryService);

            A.CallTo(() => searchQueryService.SearchAsync(A<string>.Ignored, A<SearchProperties>.Ignored)).Returns(searchResults);

            // Act
            var results = await summaryService.GetResutsList(requestUrl, searchTerm, page, pageSize).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchQueryService.SearchAsync(A<string>.Ignored, A<SearchProperties>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mapper.Map<List<SearchApiModel>>(searchResults.Results)).MustNotHaveHappened();
            Assert.Null(results);
        }
    }
}
