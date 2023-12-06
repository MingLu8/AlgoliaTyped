using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class HitsPerPage
    {
        [Fact]
        public void sets_hits_per_page()
        {
            var descriptor = new QueryDescriptor<Customer>();
            descriptor.HitsPerPage(10);
            descriptor.GetQuery().HitsPerPage.Should().Be(10);
        }
    }
}