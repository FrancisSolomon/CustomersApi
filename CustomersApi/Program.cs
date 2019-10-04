using System.Diagnostics;

using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Web;

namespace CustomersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices(s => s.AddAutofac())
                .SuppressStatusMessages(!Debugger.IsAttached)
                .ConfigureLogging(
                    l =>
                    {
                        l.ClearProviders();
                        l.SetMinimumLevel(LogLevel.Trace);
                    })
                .UseNLog();
    }
}
