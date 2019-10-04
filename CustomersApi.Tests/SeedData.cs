using System;
using System.Collections.Generic;

using CustomersApi.Models;

namespace CustomersApi.Tests
{
    internal static class SeedData
    {
        public static IEnumerable<Customer> CustomerSeedData => new List<Customer>
        {
            new Customer
            {
                Id = 1,
                IsActive = true,
                Name = "One Corp"
            },
            new Customer
            {
                Id = 2,
                IsActive = true,
                Name = "Two Corp",
                LastOrderDate = new DateTime(2019, 1, 1),
                NumberOfOrders = 4,
                TotalOrderValue = 50000M
            },
            new Customer
            {
                Id = 3,
                IsActive = false,
                Name = "Three Corp",
                LastOrderDate = new DateTime(2017, 1, 1),
                NumberOfOrders = 1,
                TotalOrderValue = 100M
            }
        };

        public static IEnumerable<Contact> ContactSeedData => new List<Contact>
        {
            new Contact
            {
                Id = 1,
                ContactType = "Billing",
                CustomerId = 2,
                FirstName = "William",
                LastName = "Fichtner"
            },
            new Contact
            {
                Id = 2,
                ContactType = "Sales",
                CustomerId = 2,
                FirstName = "Alison",
                LastName = "Smith"
            },
            new Contact
            {
                Id = 3,
                ContactType = "Billing",
                CustomerId = 3,
                FirstName = "James",
                LastName = "Harblebarble"
            }
        };

        public static IEnumerable<Address> AddressSeedData => new List<Address>
        {
            new Address
            {
                Id = 1,
                ContactId = 1,
                StreetAddress = "1 Acme St",
                City = "Acme Town",
                State = "TN",
                PostalCode = "12345",
                Country = "USA"
            },
            new Address
            {
                Id = 2,
                ContactId = 2,
                StreetAddress = "1 Acme St",
                City = "Acme Town",
                State = "TN",
                PostalCode = "12345",
                Country = "USA"
            },
            new Address
            {
                Id = 3,
                ContactId = 3,
                StreetAddress = "1000 Nowhere Lane",
                City = "Nowhereville",
                State = "NO",
                PostalCode = "99999",
                Country = "USA"
            }
        };
    }
}
