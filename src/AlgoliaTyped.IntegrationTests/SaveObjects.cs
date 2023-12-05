using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using AlgoliaTyped;
using FluentAssertions;

namespace AlgoliaTypedIntegrationTests
{

    public class SaveObjects : IDisposable
    {
        private readonly SearchClient _searchClient;
        private CustomerIndex? _index;

        public SaveObjects()
        {
            var applicationId = "FQYQ8IBGN5";
            var apiKey = "7a36900fc5bbf76a0857484917812787";
            _searchClient = new SearchClient(applicationId, apiKey);
        }

        [Fact]
        public void saves_all_objects()
        {
            _index = new CustomerIndex(_searchClient);
            _index.ConfigureIfNotExists();
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