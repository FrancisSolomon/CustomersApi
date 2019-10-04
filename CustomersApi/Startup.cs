using System.Reflection;

using Autofac;

using AutoMapper;

using CustomersApi.DataAccess;
using CustomersApi.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;

namespace CustomersApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services
                .AddApiVersioning(
                    opt =>
                    {
                        opt.ReportApiVersions = true;
                        opt.Conventions.Add(new VersionByNamespaceConvention());
                    })
                .AddSwaggerGen(
                    opt =>
                    {
                        opt.SwaggerDoc(
                            "v1",
                            new Info
                            {
                                Title = "Customers Api",
                                Version = "v1"
                            });
                    })
                .AddDbContext<CustomerDbContext>(
                    opt => opt.UseSqlServer(
                        Configuration.GetConnectionString("CustomersConnection"),
                        providerOptions => providerOptions.EnableRetryOnFailure()))
                .AddAutoMapper(typeof(Startup));
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRequestLogMiddleware();
            app.UseExceptionCaptureMiddleware();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(
                opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Customers API");
                });
        }
    }
}
