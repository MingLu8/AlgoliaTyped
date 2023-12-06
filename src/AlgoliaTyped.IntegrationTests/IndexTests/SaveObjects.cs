using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using AlgoliaTyped;
using AlgoliaTypedIntegrationTests;
using FluentAssertions;
using Xunit.Extensions.AssemblyFixture;

namespace AlgoliaTyped.IntegrationTests.IndexTests
{
    public class SaveObjects : IAssemblyFixture<AlgoliaFixture>, IDisposable
    {
        private readonly SearchClient _searchClient;
        private readonly AlgoliaFixture _fixture;
        private CustomerIndex? _index;

        public SaveObjects(AlgoliaFixture fixture)
        {
            _fixture = fixture;
            _searchClient = _fixture.SearchClient;
        }

        [Fact]
        public void saves_all_objects()
        {
            _index = new CustomerIndex(_searchClient, GetType().Name);
            _index.Configure();
            var customers = new List<Customer>
            {
                new Customer( "a", "b"),
                new Customer( "a2", "b2"),
                new Customer( "a3", "b3"),
            };

            _index.SaveObjects(customers, waitForCompletion: true);


            var resultingIndex = _searchClient.InitIndex(_index.IndexName);
            var response = resultingIndex.Search<Customer>(new Algolia.Search.Models.Search.Query("a"));
            response.Hits.Should().HaveCount(3);
        }

        public void Dispose()
        {
            _index?.Delete();
        }
    }
}