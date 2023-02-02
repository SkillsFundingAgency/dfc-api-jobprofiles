using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.AzureSearch;
using FakeItEasy;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests.AzureSearch
{
    public class AzSearchQueryConverterTests
    {
        [Fact]
        public void BuildSearchParametersRaisesArgumentNullExceptionForMissingSearchProperties()
        {
            // Arrange
            SearchProperties searchProperties = null;
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => azSearchQueryConverter.BuildSearchParameters(searchProperties));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'properties')", exceptionResult.Message);
        }

        [Fact]
        public void BuildSearchParametersReturnsSearchParametersForSearchProperties()
        {
            // Arrange
            var searchProperties = A.Fake<SearchProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var results = azSearchQueryConverter.BuildSearchParameters(searchProperties);

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public void BuildSuggestParametersReturnsSuggestParametersForSuggestProperties()
        {
            // Arrange
            var suggestProperties = A.Fake<SuggestProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var results = azSearchQueryConverter.BuildSuggestParameters(suggestProperties);

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public void ConvertToSearchResultRaisesArgumentNullExceptionForMissingSearchProperties()
        {
            // Arrange
            var fakeResults = A.Fake<IEnumerable<Azure.Search.Documents.Models.SearchResult<JobProfileIndex>>>();
            var fakeFacets = A.Fake<IDictionary<string, IList<FacetResult>>>();
            var fakeResponse = A.Fake<Response>();
            var modelledResults = SearchModelFactory.SearchResults<JobProfileIndex>(fakeResults, 0, fakeFacets, 0, fakeResponse); // maybe null response

            SearchProperties searchProperties = null;
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => azSearchQueryConverter.ConvertToSearchResult(modelledResults, searchProperties));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'properties')", exceptionResult.Message);
        }

        [Fact]
        public void ConvertToSearchResultRaisesArgumentNullExceptionForMissingResult()
        {
            // Arrange
            SearchResults<JobProfileIndex> results = null;
            var searchProperties = A.Fake<SearchProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => azSearchQueryConverter.ConvertToSearchResult(results, searchProperties));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'results')", exceptionResult.Message);
        }

        [Fact]
        public void ConvertToSearchResultReturnsSearchResultForSearchProperties()
        {
            // Arrange
            var fakeResults = A.Fake<IEnumerable<Azure.Search.Documents.Models.SearchResult<JobProfileIndex>>>();
            var facets = A.Fake<IDictionary<string, IList<FacetResult>>>();
            var fakeResponse = A.Fake<Response>();
            var modelledResults = SearchModelFactory.SearchResults<JobProfileIndex>(fakeResults, 0, facets, 0, fakeResponse); // maybe null response
            var searchProperties = A.Fake<SearchProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var finalResult = azSearchQueryConverter.ConvertToSearchResult(modelledResults, searchProperties);

            // Assert
            Assert.NotNull(finalResult);
        }

        [Fact]
        public void ConvertToSuggestionResultRaisesArgumentNullExceptionForMissingSearchProperties()
        {
            // Arrange
            var result = A.Fake<SuggestResults<JobProfileIndex>>();
            SuggestProperties suggestProperties = null;
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => azSearchQueryConverter.ConvertToSuggestionResult(result, suggestProperties));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'properties')", exceptionResult.Message);
        }

        [Fact]
        public void ConvertToSuggestionResultRaisesArgumentNullExceptionForMissingResult()
        {
            // Arrange
            SuggestResults<JobProfileIndex> result = null;
            var suggestProperties = A.Fake<SuggestProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => azSearchQueryConverter.ConvertToSuggestionResult(result, suggestProperties));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'result')", exceptionResult.Message);
        }

        [Fact]
        public void ConvertToSuggestionResultReturnsSuggestionResultForSearchProperties()
        {
            // Arrange
            var fakeResult = A.Fake<SuggestResults<JobProfileIndex>>();
            var fakeSuggestProperties = A.Fake<SuggestProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var finalResult = azSearchQueryConverter.ConvertToSuggestionResult(fakeResult, fakeSuggestProperties);

            // Assert
            Assert.NotNull(finalResult);
        }
    }
}