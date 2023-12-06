using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class SearchQuery
    {
        [Fact]
        public void sets_search_query()
        {
            var descriptor = new QueryDescriptor<Customer>();
            descriptor.SearchQuery("xyz");
            descriptor.GetQuery().SearchQuery.Should().Be("xyz");
        }
    }
}