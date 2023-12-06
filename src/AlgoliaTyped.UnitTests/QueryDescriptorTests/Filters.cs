using FluentAssertions;

namespace AlgoliaTyped.UnitTests.IndexTests
{
    public class Filters
    {
        private readonly QueryDescriptor<Customer> _descriptor;

        public Filters()
        {
            _descriptor = new QueryDescriptor<Customer>();
        }

        [Fact]
        public void with_equality_operator()
        {
            _descriptor.Filters(a => a.FirstName == "xyz");
            _descriptor.GetQuery().Filters.Should().Be("(firstName:'xyz')");
        }

        [Fact]
        public void with_not_equality_operator()
        {
            _descriptor.Filters(a => a.LastName != "abc");
            _descriptor.GetQuery().Filters.Should().Be("(NOT lastName:'abc')");
        }

        [Fact]
        public void with_great_than_operator()
        {
            _descriptor.Filters(a => a.Age > 20);
            _descriptor.GetQuery().Filters.Should().Be("(age>20)");
        }

        [Fact]
        public void with_great_equal_than_operator()
        {
            _descriptor.Filters(a => a.Age >= 20);
            _descriptor.GetQuery().Filters.Should().Be("(age>=20)");
        }

        [Fact]
        public void with_less_than_operator()
        {
            _descriptor.Filters(a => a.Age < 20);
            _descriptor.GetQuery().Filters.Should().Be("(age<20)");
        }
        [Fact]
        public void with_less_equal_than_operator()
        {
            _descriptor.Filters(a => a.Age <= 20);
            _descriptor.GetQuery().Filters.Should().Be("(age<=20)");
        }

        [Fact]
        public void with_and_not_operator()
        {
            _descriptor.Filters(a => a.FirstName == "xyz" && a.Age != 20);
            _descriptor.GetQuery().Filters.Should().Be("(firstName:'xyz') AND (NOT age:20)");
        }

        [Fact]
        public void with_or_operator()
        {
            _descriptor.Filters(a => a.FirstName == "abc" || a.LastName == "xyz");
            _descriptor.GetQuery().Filters.Should().Be("(firstName:'abc') OR (lastName:'xyz')");
        }

        [Fact]
        public void with_multiple_filters_calls()
        {
            _descriptor.Filters(a => a.FirstName == "aaa");
            _descriptor.Filters(a => a.LastName == "bbb");
            _descriptor.Filters(a => a.LastName == "ccc", true);
            _descriptor.GetQuery().Filters.Should().Be("(firstName:'aaa') AND (lastName:'bbb') OR (lastName:'ccc')");
        }
    }
}