using Companies.API;
using Companies.API.Data;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Integration.Tests
{
    public class CustomWebApplicationFactory<T> : WebApplicationFactory<Program>, IDisposable
    {
        public CompaniesContext Context { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                var serviceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CompaniesContext>));

                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                }

                services.AddDbContext<CompaniesContext>(options =>
                {
                    options.UseInMemoryDatabase("PlaceholderDataBase");
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var provider = scope.ServiceProvider;

                Context = provider.GetRequiredService<CompaniesContext>();

                Context.Companies.AddRange(
                    [
                    new Company()
                    {
                        Name = "TestCompanyName",
                        Address = "TestAddress",
                        Country = "TestCountry",
                        Employees =
                        [
                            new ApplicationUser
                            {
                                Age = 50,
                                Name = "TestName",
                                Position = "Developer"
                            }
                        ]
                    }
                    ]);

                Context.SaveChanges();
            });
        }

        public override ValueTask DisposeAsync()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            return base.DisposeAsync();
        }
    }
}
