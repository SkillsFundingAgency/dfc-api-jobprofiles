using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.AzureSearch;
using FakeItEasy;
using Microsoft.Azure.Search.Models;
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
            var searchResults = A.Fake<IList<Microsoft.Azure.Search.Models.SearchResult<JobProfileIndex>>>();
            var facets = A.Fake<IDictionary<string, IList<FacetResult>>>();
            var result = new DocumentSearchResult<JobProfileIndex>(searchResults, 0, 0, facets, null);
            SearchProperties searchProperties = null;
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => azSearchQueryConverter.ConvertToSearchResult(result, searchProperties));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'properties')", exceptionResult.Message);
        }

        [Fact]
        public void ConvertToSearchResultRaisesArgumentNullExceptionForMissingResult()
        {
            // Arrange
            DocumentSearchResult<JobProfileIndex> result = null;
            var searchProperties = A.Fake<SearchProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => azSearchQueryConverter.ConvertToSearchResult(result, searchProperties));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'result')", exceptionResult.Message);
        }

        [Fact]
        public void ConvertToSearchResultReturnsSearchResultForSearchProperties()
        {
            // Arrange
            var searchResults = A.Fake<IList<Microsoft.Azure.Search.Models.SearchResult<JobProfileIndex>>>();
            var facets = A.Fake<IDictionary<string, IList<FacetResult>>>();
            var result = new DocumentSearchResult<JobProfileIndex>(searchResults, 0, 0, facets, null);
            var searchProperties = A.Fake<SearchProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var results = azSearchQueryConverter.ConvertToSearchResult(result, searchProperties);

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public void ConvertToSuggestionResultRaisesArgumentNullExceptionForMissingSearchProperties()
        {
            // Arrange
            var result = A.Fake<DocumentSuggestResult<JobProfileIndex>>();
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
            DocumentSuggestResult<JobProfileIndex> result = null;
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
            var result = A.Fake<DocumentSuggestResult<JobProfileIndex>>();
            var suggestProperties = A.Fake<SuggestProperties>();
            var azSearchQueryConverter = new AzSearchQueryConverter();

            // Act
            var results = azSearchQueryConverter.ConvertToSuggestionResult(result, suggestProperties);

            // Assert
            Assert.NotNull(results);
        }
    }
}