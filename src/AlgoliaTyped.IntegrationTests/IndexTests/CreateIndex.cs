using Algolia.Search.Clients;
using AlgoliaTyped;
using AlgoliaTypedIntegrationTests;
using FluentAssertions;
using Xunit.Extensions.AssemblyFixture;

namespace AlgoliaTyped.IntegrationTests.IndexTests
{

    public class CreateIndex : IAssemblyFixture<AlgoliaFixture>, IDisposable
    {
        private readonly SearchClient _searchClient;
        private readonly AlgoliaFixture _fixture;
        private CustomerIndex? _index;

        public CreateIndex(AlgoliaFixture fixture)
        {
            _fixture = fixture;
            _searchClient = _fixture.SearchClient;
        }

        [Fact]
        public void create_index_with_settings()
        {
            _index = new CustomerIndex(_searchClient, GetType().Name);
            _index.IndexName.Should().Be(GetType().Name);

            _index.Configure();

            var resultingIndex = _searchClient.InitIndex(_index.IndexName);
            resultingIndex.GetSettings().SearchableAttributes.Should().BeEquivalentTo("objectID", "firstName", "lastName");
        }

        public void Dispose()
        {
            _index?.Delete();
        }
    }
}