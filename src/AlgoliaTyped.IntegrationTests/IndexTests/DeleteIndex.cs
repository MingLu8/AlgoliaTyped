using Algolia.Search.Clients;
using AlgoliaTypedIntegrationTests;
using FluentAssertions;
using Xunit.Extensions.AssemblyFixture;

namespace AlgoliaTyped.IntegrationTests.IndexTests
{
    public class DeleteIndex : IAssemblyFixture<AlgoliaFixture>
    {
        private readonly SearchClient _searchClient;
        private readonly AlgoliaFixture _fixture;
        private CustomerIndex? _index;

        public DeleteIndex(AlgoliaFixture fixture)
        {
            _fixture = fixture;
            _searchClient = _fixture.SearchClient;
        }

        [Fact]
        public void removes_index_from_algolia()
        {
            _index = new CustomerIndex(_searchClient, GetType().Name);
            _index.Configure();
            _index.Delete();
            var exists = _index.InternalSearchIndex.Exists();
            exists.Should().BeFalse();

        }
    }
}