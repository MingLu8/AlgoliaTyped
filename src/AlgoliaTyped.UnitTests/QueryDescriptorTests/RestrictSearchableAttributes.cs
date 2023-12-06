using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class RestrictSearchableAttributes
    {
        [Fact]
        public void RestrictSearchableAttributes_with_simple_value()
        {
            var descriptor = new QueryDescriptor<Customer>();
            descriptor.RestrictSearchableAttributes(a => a.FirstName);
            descriptor.GetQuery().RestrictSearchableAttributes.Should().BeEquivalentTo("firstName");
        }
    }
}