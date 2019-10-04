using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CustomersApi.Tests.Fixtures
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;

        public TestServerFixture(
            Action<ContainerBuilder> configureContainer = null,
            Action<IServiceCollection> testServiceConfiguration = null,
            ICollection<string> configurationOptions = null)
        {
            configurationOptions = configurationOptions ?? new List<string>();
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(s =>
                {
                    s.AddAutofac();
                })
                .ConfigureTestServices(s =>
                {
                    // remove db context from service collection
                    var existingContext = s
                        .Single(d => d.ServiceType.IsSubclassOf(typeof(DbContext)));
                    s.Remove(existingContext);

                    testServiceConfiguration?.Invoke(s);
                })
                .ConfigureTestContainer<ContainerBuilder>(c =>
                {
                    configureContainer?.Invoke(c);
                });

            builder.UseEnvironment("Test");

            _testServer = new TestServer(builder);

            Services = _testServer.Host.Services;
            Client = _testServer.CreateClient();
            Client.BaseAddress = new Uri(@"https://localhost");
        }

        public HttpClient Client { get; }

        public IServiceProvider Services { get; }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}
