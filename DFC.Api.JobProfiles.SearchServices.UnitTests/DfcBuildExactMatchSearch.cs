using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using FluentAssertions;
using Xunit;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests
{
    public class DfcBuildExactMatchSearch
    {
        [Theory]
        [InlineData("", "", false, "")]
        [InlineData("term1", "term1", false, "term1 term1")]
        [InlineData("term1", "term1", true, "term1")]
        [InlineData("term1 term2", "term3", false, "\"term1 term2\" term3")]
        [InlineData("term1 term2", "term3", true, "term1 term2")]
        public void BuildExactMatchTest(string searchTerm, string partialSearchTerm, bool useRawSearchTerm, string expected)
        {
            var queryBuilder = new DfcSearchQueryBuilder();
            var result = queryBuilder.BuildExactMatchSearch(searchTerm, partialSearchTerm, new SearchProperties { UseRawSearchTerm = useRawSearchTerm });
            result.Should().Be(expected);
        }
    }
}
