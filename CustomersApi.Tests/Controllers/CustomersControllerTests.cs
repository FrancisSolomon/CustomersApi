using System.Collections.Generic;
using System.Linq;
using System.Net;

using Autofac;

using CustomersApi.DataAccess;
using CustomersApi.Tests.Fixtures;
using CustomersApi.V1.Models;

using FluentAssertions;

using Newtonsoft.Json;

using NUnit.Framework;

namespace CustomersApi.Tests.Controllers
{
    [TestFixture]
    public class CustomersControllerTests
    {
        private TestServerFixture _testServer;

        private CustomersDataFixture _customersDataFixture;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            _customersDataFixture = new CustomersDataFixture();
            _testServer = new TestServerFixture(BuildTestContainerBuilder);
        }

        [OneTimeTearDown]
        public void TearDownFixture()
        {
            _customersDataFixture.Dispose();
            _testServer.Dispose();
        }

        [Test]
        public void GetById_ShouldReturnCustomer_WhenValidIdSupplied()
        {
            const long customerId = 2;
            var expectedCustomer = SeedData.CustomerSeedData.Single(c => c.Id == customerId);

            var response = _testServer.Client.GetAsync($"v1/customers/{customerId}").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<DtoCustomer>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            customer.CustomerId.Should().Be(expectedCustomer.Id);
            customer.Name.Should().Be(expectedCustomer.Name);
        }

        [Test]
        public void GetById_ShouldReturnNotFound_WhenInvalidIdSupplied()
        {
            const long badCustomerId = 4;

            var response = _testServer.Client.GetAsync($"v1/customers/{badCustomerId}").Result;

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestCase("Name=corp", new long[] { 1, 2 })]
        [TestCase("Name=one", new long[] { 1 })]
        [TestCase("Name=three", new long[] { })]
        [TestCase("Name=three&IncludeInactive=true", new long[] { 3 })]
        [TestCase("IncludeInactive=true", new long[] { 1, 2, 3 })]
        [TestCase("MinimumSpend=1", new long[] { 2 })]
        public void Get_ShouldReturnCorrectFilteredList_WhenFilterSpecified(string urlFilter, long[] expectedIds)
        {
            var response = _testServer.Client.GetAsync($"/v1/customers?{urlFilter}").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var customers = JsonConvert.DeserializeObject<List<DtoCustomer>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            customers.Select(i => i.CustomerId)
                .Should()
                .BeEquivalentTo(
                    SeedData.CustomerSeedData.Where(x => expectedIds.Contains(x.Id)).Select(d => d.Id));
        }

        [Test]
        public void Get_ShouldIncludeContacts_WhenSpecifiedInQuery()
        {
            var response = _testServer.Client.GetAsync($"v1/customers?Name=two&Include=Contacts").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var customers = JsonConvert.DeserializeObject<List<DtoCustomer>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            customers.Should().ContainSingle();

            var customerTwo = customers.Single();
            customerTwo.Contacts.Should().NotBeNull();
            customerTwo.Contacts.Should().HaveCount(2);
            customerTwo.Contacts.Should().ContainSingle(c => c.FirstName == "Alison");
            customerTwo.Contacts.Should().ContainSingle(c => c.FirstName == "William");
        }

        [Test]
        public void Get_Should_IncludeAddresses_WhenSpecifiedInQuery()
        {
            var response = _testServer.Client.GetAsync($"v1/customers?Name=two&Include=Contacts&Include=Addresses").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var customers = JsonConvert.DeserializeObject<List<DtoCustomer>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            customers.Should().ContainSingle();

            var customerTwo = customers.Single();
            customerTwo.Contacts.Should().NotBeNull();
            customerTwo.Contacts.Should().HaveCount(2);

            var customerTwoContact = customerTwo.Contacts.First(c => c.ContactId == 1);
            customerTwoContact.Addresses.Should().NotBeNull();
            customerTwoContact.Addresses.Should().ContainSingle();
            customerTwoContact.Addresses.Should().ContainSingle(c => c.AddressId == 1);
        }

        private void BuildTestContainerBuilder(ContainerBuilder builder)
        {
            builder.RegisterInstance(_customersDataFixture.Context).As<CustomerDbContext>();
        }
    }
}
