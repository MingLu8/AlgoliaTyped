using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class Facets
    {
        [Fact]
        public void sets_facets_to_start_character()
        {
            var descriptor = new QueryDescriptor<Customer>();
            descriptor.Facets(a=>a.FirstName);
            descriptor.GetQuery().Facets.Should().BeEquivalentTo("firstName");
        }
    }
}