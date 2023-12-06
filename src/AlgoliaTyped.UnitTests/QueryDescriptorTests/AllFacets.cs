using Algolia.Search.Clients;
using AlgoliaTyped;
using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class AllFacets
    {
        [Fact]
        public void sets_facets_to_start_character()
        {
            var descriptor = new QueryDescriptor<Customer>();
            descriptor.AllFacets();
            descriptor.GetQuery().Facets.Should().BeEquivalentTo( "*");
        }
    }
}