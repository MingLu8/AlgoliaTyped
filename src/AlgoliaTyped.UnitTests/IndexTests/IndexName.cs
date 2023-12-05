using Algolia.Search.Clients;
using AlgoliaTyped;
using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class IndexName
    {
        private readonly SearchClient _searchClient;

        public IndexName()
        {
            var applicationId = "FQYQ8IBGN5";
            var apiKey = "7a36900fc5bbf76a0857484917812787";
            _searchClient = new SearchClient(applicationId, apiKey);
        }

        [Fact]
        public void is_default_type_name()
        {
            var index = new TypedIndex<IndexName>(_searchClient);
            index.IndexName.Should().Be(typeof(IndexName).Name);
        }

        [Fact]
        public void is_overridden_index_name()
        {
            var index = new CustomerIndex(_searchClient);
            index.IndexName.Should().Be("my_customer");
        }
    }

    public record Customer(string firstName, string lastName);
    public class CustomerIndex : TypedIndex<Customer>
    {
        public override string IndexName => "my_customer";
        public CustomerIndex(ISearchClient searchClient) : base(searchClient)
        {
        }
    }
}