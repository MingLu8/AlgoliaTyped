﻿namespace AlgoliaTypedIntegrationTests
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
}