﻿using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.UnitTests.Utilities;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests
{
    public class JobProfileSearchManipulatorTests
    {
        [Theory]
        [InlineData("dummyTitletest5", 1, "dummyTitletest5")]
        [InlineData("dummyTitletest5", 2, "dummyTitletest1")]
        [InlineData(null, 1, "dummyTitletest1")]
        public void ReorderTitleTest(string searchTitle, int page, string expectedFirstResult)
        {
            var mannipulator = new JobProfileSearchManipulator();
            Data.AzureSearch.Models.SearchResult<JobProfileIndex> data = new Data.AzureSearch.Models.SearchResult<JobProfileIndex>
            {
                Results = DummyJobProfileIndex.GenerateJobProfileResultItemDummyCollection("test", 10, 1),
            };

            SearchProperties searchProperties = new SearchProperties
            {
                Page = page,
            };

            var result = mannipulator.Reorder(data, searchTitle, searchProperties);
            result.Results.First().ResultItem.Title.Should().Be(expectedFirstResult);
        }

        [Theory]
        [InlineData("dummyAlternativeTitle5", 1, "dummyAlternativeTitle5")]
        [InlineData("dummyAlternativeTitle5", 2, "dummyAlternativeTitle1")]
        [InlineData(null, 1, "dummyAlternativeTitle1")]
        public void ReorderAlternativeTitleTest(string searchTitle, int page, string expectedFirstResult)
        {
            var mannipulator = new JobProfileSearchManipulator();
            Data.AzureSearch.Models.SearchResult<JobProfileIndex> data = new Data.AzureSearch.Models.SearchResult<JobProfileIndex>
            {
                Results = DummyJobProfileIndex.GenerateJobProfileResultItemDummyCollection("Test", 10, 1),
            };

            SearchProperties searchProperties = new SearchProperties
            {
                Page = page,
            };

            var result = mannipulator.Reorder(data, searchTitle, searchProperties);
            result.Results.First().ResultItem.AlternativeTitle.First().Should().Be(expectedFirstResult);
        }

        [Fact]
        public void ReorderNullResultTest()
        {
            var mannipulator = new JobProfileSearchManipulator();
            Data.AzureSearch.Models.SearchResult<JobProfileIndex> data = new Data.AzureSearch.Models.SearchResult<JobProfileIndex>
            {
                Results = null,
            };

            SearchProperties searchProperties = new SearchProperties
            {
                Page = 1,
            };

            var result = mannipulator.Reorder(data, "test", searchProperties);
            result.Results.Should().BeEquivalentTo(Enumerable.Empty<Data.AzureSearch.Models.SearchResultItem<JobProfileIndex>>());
        }

        [Fact]
        public void ReorderNullDataTest()
        {
            var mannipulator = new JobProfileSearchManipulator();
            SearchProperties searchProperties = new SearchProperties
            {
                Page = 1,
            };

            var result = mannipulator.Reorder(null, "test", searchProperties);
            result.Results.Should().BeEquivalentTo(Enumerable.Empty<Data.AzureSearch.Models.SearchResultItem<JobProfileIndex>>());
        }

        [Fact]
        public void ReorderNullEverythingTest()
        {
            var mannipulator = new JobProfileSearchManipulator();
            var result = mannipulator.Reorder(null, "test", null);
            result.Results.Should().BeEquivalentTo(Enumerable.Empty<Data.AzureSearch.Models.SearchResultItem<JobProfileIndex>>());
        }

        [Theory]
        [InlineData("Order Picker", "Order Picker")]
        [InlineData("Packer", "Packer")]
        public void TitleAsPriorityTest(string searchTerm, string expectedFirstResult)
        {
            var mannipulator = new JobProfileSearchManipulator();
            Data.AzureSearch.Models.SearchResult<JobProfileIndex> data = new Data.AzureSearch.Models.SearchResult<JobProfileIndex>
            {
                Results = DummyJobProfileIndex.GenerateJobProfileResultItemDummyCollectionWithOrderPicker("Test"),
            };

            SearchProperties searchProperties = new SearchProperties
            {
                Page = 1,
            };

            var result = mannipulator.Reorder(data, searchTerm, searchProperties);
            result.Results.First().ResultItem.Title.Should().Be(expectedFirstResult);
        }

        [Theory]
        [InlineData("*", "cleanSearch", "partialSearchTerm", null, "*")]
        [InlineData("*", "cleanSearch", "partialSearchTerm", false, "*")]
        [InlineData("*", "cleanSearch", "partialSearchTerm", true, "*")]
        [InlineData("test", "cleanSearch", "partialSearchTerm", null, "Title:(partialsearchterm) AlternativeTitle:(partialsearchterm) TitleAsKeyword:\"test\" AltTitleAsKeywords:\"test\" cleanSearch")]
        [InlineData("test", "cleanSearch", "partialSearchTerm", false, "Title:(partialsearchterm) AlternativeTitle:(partialsearchterm) TitleAsKeyword:\"test\" AltTitleAsKeywords:\"test\" cleanSearch")]
        [InlineData("test", "cleanSearch", "partialSearchTerm", true, "cleanSearch")]
        public void BuildSearchExpressionForRawTest(string searchTerm, string cleanedSearchTerm, string partialTermToSearch, bool? isRaw, string expectation)
        {
            var prop = isRaw is null ? null : new SearchProperties { UseRawSearchTerm = isRaw.Value };
            var mannipulator = new JobProfileSearchManipulator();
            var result = mannipulator.BuildSearchExpression(searchTerm, cleanedSearchTerm, partialTermToSearch, prop);

            result.Should().Be(expectation);
        }
    }
}
