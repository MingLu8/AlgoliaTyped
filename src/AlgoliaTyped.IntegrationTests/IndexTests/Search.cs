using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using AlgoliaTyped;
using AlgoliaTypedIntegrationTests;
using FluentAssertions;
using Xunit.Extensions.AssemblyFixture;

namespace AlgoliaTyped.IntegrationTests.IndexTests
{
    public class Search : IAssemblyFixture<AlgoliaFixture>, IDisposable
    {
        private readonly SearchClient _searchClient;
        private readonly AlgoliaFixture _fixture;
        private CustomerIndex? _index;

        public Search(AlgoliaFixture fixture)
        {
            _fixture = fixture;
            _searchClient = _fixture.SearchClient;
        }

        [Fact]
        public void returns_search_response()
        {
            _index = new CustomerIndex(_searchClient, GetType().Name);
            _index.Configure();
            var customers = new List<Customer>
            {
                new Customer( "a", "b", 12),
                new Customer( "a2", "b2", 22),
                new Customer( "a3", "b3", 32),
            };

            _index.SaveObjects(customers, waitForCompletion: true);

            var response = _index.Search(desc=> desc.Filters(a=>a.FirstName == "a").GetQuery());
            response.Hits.Should().HaveCount(1);
        }

        public void Dispose()
        {
            _index?.Delete();
        }
    }
}