using AutoMapper;
using DFC.Api.JobProfiles.Data.ApiModels;
using DFC.Api.JobProfiles.Data.ApiModels.Search;
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
            const int expectedItemCount = 2;
            var searchResults = new SearchResult<JobProfileIndex>()
            {
                Results = A.CollectionOfFake<SearchResultItem<JobProfileIndex>>(expectedItemCount),
                Count = expectedItemCount,
            };
            var expectedResult = A.Fake<SearchApiModel>();
            expectedResult.Count = expectedItemCount;
            expectedResult.Results = A.CollectionOfFake<SearchItemApiModel>(expectedItemCount);
            var summaryService = new SearchService(mapper, searchQueryService);

            A.CallTo(() => searchQueryService.SearchAsync(A<string>.Ignored, A<SearchProperties>.Ignored)).Returns(searchResults);
            A.CallTo(() => mapper.Map<SearchApiModel>(searchResults)).Returns(expectedResult);

            // Act
            var results = await summaryService.GetResutsList(requestUrl, searchTerm, page, pageSize).ConfigureAwait(false);

            // Assert
            A.CallTo(() => searchQueryService.SearchAsync(A<string>.Ignored, A<SearchProperties>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mapper.Map<SearchApiModel>(searchResults)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult.Count, results.Count);
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
            A.CallTo(() => mapper.Map<SearchApiModel>(A< SearchResult<JobProfileIndex>>.Ignored)).MustNotHaveHappened();
            Assert.Null(results);
        }
    }
}
