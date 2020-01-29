using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using FakeItEasy;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests
{
    public class DfcSearchQueryServiceTests
    {
        private const string CleanedSearchTerm = "cleanedSearchTerm";
        private const string PartialTermToSearch = "partialTermToSearch";
        private const string TrimmedResults = "trimmed result";
        private const string SearchTerm = "searchTerm";

        [Fact]
        public async Task SearchAsyncTest()
        {
            //Arrange
            var fakeIndexClient = A.Fake<ISearchIndexClient>(o => o.Strict());
            var fakeQueryBuilder = A.Fake<ISearchQueryBuilder>(o => o.Strict());
            var fakeQueryConverter = A.Fake<IAzSearchQueryConverter>(o => o.Strict());
            var fakeDocumentsOperation = A.Fake<IDocumentsOperations>();
            var dummySearchProperty = A.Dummy<SearchProperties>();
            var dummySearchParameters = A.Dummy<SearchParameters>();
            var dummySearchResult = A.Dummy<Data.AzureSearch.Models.SearchResult<JobProfileIndex>>();
            var fakeManipulator = A.Fake<ISearchManipulator<JobProfileIndex>>();
            var dummySearchIndexClientFactory = A.Fake<ISearchIndexClientFactory>();

            //Configure
            A.CallTo(() => dummySearchIndexClientFactory.GetSearchIndexClient()).Returns(fakeIndexClient);
            A.CallTo(() => fakeQueryBuilder.RemoveSpecialCharactersFromTheSearchTerm(A<string>._, A<SearchProperties>._)).Returns(CleanedSearchTerm);
            A.CallTo(() => fakeQueryBuilder.BuildContainPartialSearch(A<string>._, A<SearchProperties>._)).Returns(PartialTermToSearch);
            A.CallTo(() => fakeQueryBuilder.TrimCommonWordsAndSuffixes(A<string>._, A<SearchProperties>._)).Returns(TrimmedResults);
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).Returns(dummySearchParameters);
            A.CallTo(() => fakeIndexClient.Documents).Returns(fakeDocumentsOperation);
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).Returns(dummySearchResult);
            A.CallTo(() => fakeManipulator.BuildSearchExpression(A<string>._, A<string>._, A<string>._, A<SearchProperties>._)).Returns(nameof(fakeManipulator.BuildSearchExpression));

            //Act
            var searchService = new DfcSearchQueryService<JobProfileIndex>(fakeQueryConverter, fakeQueryBuilder, fakeManipulator, dummySearchIndexClientFactory);
            await searchService.SearchAsync("searchTerm", dummySearchProperty).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeQueryBuilder.RemoveSpecialCharactersFromTheSearchTerm(A<string>.That.IsEqualTo(SearchTerm), A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryBuilder.TrimCommonWordsAndSuffixes(A<string>.That.IsEqualTo(CleanedSearchTerm), A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryBuilder.BuildContainPartialSearch(A<string>.That.IsEqualTo(TrimmedResults), A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeIndexClient.Documents).MustHaveHappened();
            A.CallTo(() => fakeDocumentsOperation.SearchWithHttpMessagesAsync<JobProfileIndex>(A<string>.That.IsEqualTo(nameof(fakeManipulator.BuildSearchExpression)), A<SearchParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData("*", "*")]
        [InlineData("term1", "Title:(/.*term1.*/) AlternativeTitle:(/.*term1.*/) TitleAsKeyword:\"term1\" AltTitleAsKeywords:\"term1\" term1")]
        [InlineData("term1 term2", "Title:(/.*term1.*/ /.*term2.*/) AlternativeTitle:(/.*term1.*/ /.*term2.*/) TitleAsKeyword:\"term1 term2\" AltTitleAsKeywords:\"term1 term2\" term1 term2")]
        public async Task SearchActualBuilderAsyncTest(string searchTerm, string expectedComputedSearchTerm)
        {
            //Arrange
            //Reals
            var actualQueryBuilder = new DfcSearchQueryBuilder();
            var actualManipulator = new JobProfileSearchManipulator();

            //Fakes
            var fakeIndexClient = A.Fake<ISearchIndexClient>(o => o.Strict());
            var fakeQueryConverter = A.Fake<IAzSearchQueryConverter>(o => o.Strict());
            var fakeDocumentsOperation = A.Fake<IDocumentsOperations>();
            var dummySearchProperty = A.Dummy<SearchProperties>();
            var dummySearchParameters = A.Dummy<SearchParameters>();
            var dummySearchResult = A.Dummy<Data.AzureSearch.Models.SearchResult<JobProfileIndex>>();
            var dummySearchIndexClientFactory = A.Fake<ISearchIndexClientFactory>();

            //Configure
            A.CallTo(() => dummySearchIndexClientFactory.GetSearchIndexClient()).Returns(fakeIndexClient);
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).Returns(dummySearchParameters);
            A.CallTo(() => fakeIndexClient.Documents).Returns(fakeDocumentsOperation);
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).Returns(dummySearchResult);

            //Act
            var searchService = new DfcSearchQueryService<JobProfileIndex>(fakeQueryConverter, actualQueryBuilder, actualManipulator, dummySearchIndexClientFactory);
            await searchService.SearchAsync(searchTerm, dummySearchProperty).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeQueryConverter.BuildSearchParameters(A<SearchProperties>._)).MustHaveHappened();
            A.CallTo(() => fakeIndexClient.Documents).MustHaveHappened();
            A.CallTo(() => fakeDocumentsOperation.SearchWithHttpMessagesAsync<JobProfileIndex>(A<string>.That.Contains(expectedComputedSearchTerm), A<SearchParameters>._, A<SearchRequestOptions>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => fakeQueryConverter.ConvertToSearchResult(A<DocumentSearchResult<JobProfileIndex>>._, A<SearchProperties>._)).MustHaveHappened();
        }
    }
}