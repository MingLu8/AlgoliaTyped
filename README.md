# AlgoliaTyped
Note: this library is still work in progress, use it as your own risk!
Algolia C# library is not very easy to use, it requires constantly looking at their documentations for their syntax and convensions, 
which is difficult to remember and error prone. The objectives of this library is to hide away this pain with simple fluent api to setup indices and query searches.

# Get Started
- Frist, define index like so:
  ``` C#
     public class CustomerIndex : TypedIndex<Customer>
    {        
        public CustomerIndex(ISearchClient searchClient) : base(searchClient)
        {
        }

        public override IndexSettings ConfigureIndexSettings(IIndexSettingsConfigurator<Customer> indexSettingsConfigurator)
        {
            var settings = indexSettingsConfigurator
                .SetAllAttributesSearchable()
                .SetAllAttributesFaceted(FacetType.Searchable)
                .RemoveFacetedAttributes(a=>a.FirstName)
                .Configure();
            return settings;
        }      
    }
  ```
- Then, add records like so:
  ```C#
    var index = new CustomerIndex(_searchClient);
    index.ConfigureIfNotExists();
    var customers = new List<Customer>
    {
        new Customer( "a", "b"),
        new Customer( "a2", "b2"),
        new Customer( "a3", "b3"),
    };

    index.SaveObjects(customers, waitForCompletion: true);
  ```
- Then, search records like so:
  ```C#
    index.Search(a => a.SearchQuery("b").Filters(b => b.FirstName == "a"));
  ```