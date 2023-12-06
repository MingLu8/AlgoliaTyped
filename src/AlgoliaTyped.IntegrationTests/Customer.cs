namespace AlgoliaTypedIntegrationTests
{
    public class Customer
    {
        public string ObjectID { get; set; } = Guid.NewGuid().ToString("D");
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Customer(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
    }
}