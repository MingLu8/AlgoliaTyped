using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class Page
    {
        [Fact]
        public void sets_page()
        {
            var descriptor = new QueryDescriptor<Customer>();
            descriptor.Page(1);
            descriptor.GetQuery().Page.Should().Be(1);
        }
    }
}