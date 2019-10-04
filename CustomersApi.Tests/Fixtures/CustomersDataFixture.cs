using System;

using CustomersApi.DataAccess;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CustomersApi.Tests.Fixtures
{
    public class CustomersDataFixture : IDisposable
    {
        public CustomersDataFixture()
        {
            var builder = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            Context = new CustomerDbContext(builder.Options);
            Context.Database.EnsureCreated();

            SeedContext();
        }

        public CustomerDbContext Context { get; }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

        private void SeedContext()
        {
            Context.Customers.AddRange(SeedData.CustomerSeedData);
            Context.Contacts.AddRange(SeedData.ContactSeedData);
            Context.Addresses.AddRange(SeedData.AddressSeedData);

            Context.SaveChanges();
        }
    }
}
