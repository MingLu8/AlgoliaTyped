using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using AlgoliaTyped;
using FluentAssertions;

namespace AlgoliaTypedIntegrationTests
{
    public class Customer
    {
        public string ObjectID { get; set; } = Guid.NewGuid().ToString("D");
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class CreateIndex : IDisposable
    {
        private readonly SearchClient _searchClient;
        private TypedIndex<Customer>? _index;

        public CreateIndex()
        {
            var applicationId = "FQYQ8IBGN5";
            var apiKey = "7a36900fc5bbf76a0857484917812787";
            _searchClient = new SearchClient(applicationId, apiKey);
        }

        [Fact]
        public void create_index_with_settings()
        {
            _index = new CustomerIndex(_searchClient);
            _index.IndexName.Should().Be("my_customer");

            _index.ConfigureIfNotExists();

            var resultingIndex = _searchClient.InitIndex(_index.IndexName);
            resultingIndex.GetSettings().SearchableAttributes.Should().BeEquivalentTo("firstName", "lastName");
        }

        public void Dispose()
        {
            _index?.Delete();
        }
    }

    public class CustomerIndex : TypedIndex<Customer>
    {
        public override string IndexName => "my_customer";
     
        public CustomerIndex(ISearchClient searchClient) : base(searchClient)
        {
        }

        public override IndexSettings ConfigureIndexSettings(IIndexSettingsConfigurator<Customer> indexSettingsConfigurator)
        {
            var settings = indexSettingsConfigurator.SetAllFieldsSearchable().GetIndexSettings();
            settings.AttributesForFaceting = new List<string> { "searchable(lastName)", "searchable(firstName)" };
            return settings;
        }      
    }
}