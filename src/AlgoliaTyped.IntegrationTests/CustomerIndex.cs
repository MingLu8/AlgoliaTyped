using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using AlgoliaTypedIntegrationTests;

namespace AlgoliaTyped.IntegrationTests
{
    public class CustomerIndex : TypedIndex<Customer>
    {
        public CustomerIndex(ISearchClient searchClient, string indexName = "my_customer") : base(searchClient, indexName)
        {
        }

        public override IndexSettings ConfigureIndexSettings(IIndexSettingsConfigurator<Customer> indexSettingsConfigurator)
        {
            var settings = indexSettingsConfigurator
                .SetAllAttributesSearchable()
                .SetAllAttributesFaceted(FacetType.Searchable)
                .Configure();
            return settings;
        }
    }
}