using Algolia.Search.Clients;
using AlgoliaTyped;
using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class IndexName
    {
        [Fact]
        public void is_default_type_name()
        {
            var index = new TypedIndex<IndexName>(new SearchClient("x", "y"));
            index.IndexName.Should().Be(typeof(IndexName).Name);
        }

        [Fact]
        public void is_overridden_index_name()
        {
            var index = new CustomerIndex(new SearchClient("x", "y"));
            index.IndexName.Should().Be("my_customer");
        }
    }

    public record Customer(string FirstName, string LastName, int Age);
    public class CustomerIndex : TypedIndex<Customer>
    {
        public CustomerIndex(ISearchClient searchClient, string indexName = "my_customer") : base(searchClient, indexName)
        {
        }
    }
}