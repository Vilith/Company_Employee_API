using Bogus;
using Companies.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Companies.API.Data
{
    public static class SeedData
    {
        private static UserManager<ApplicationUser> userManager;
        private static RoleManager<IdentityRole> roleManager;
        private static IConfiguration configuration;
        private const string employeeRole = "Employee";
        private const string adminRole = "Admin";

        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<CompaniesContext>();

                await db.Database.MigrateAsync();
                if (await db.Companies.AnyAsync())
                {
                    return; // Database has been seeded
                }

                userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                configuration = serviceProvider.GetRequiredService<IConfiguration>();

                try
                {
                    await CreateRolesAsync(new[] { adminRole, employeeRole });


                    var companies = GenerateCompanies(30);
                    db.AddRange(companies);
                    await GenerateEmployeesAsync(300, companies);

                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }

        private static async Task CreateRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    continue;                    
                }
                
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }
        }

        private static List<Company> GenerateCompanies(int nrOfCompanies)
        {
            var faker = new Faker<Company>("sv").Rules((f, c) =>
            {
                c.Name = f.Company.CompanyName();
                c.Address = $"{f.Address.StreetAddress()}, {f.Address.City()}";
                c.Country = f.Address.Country();
                //c.Employees = GenerateEmployees(f.Random.Int(min: 2, max: 10));

            });

            return faker.Generate(nrOfCompanies);
        }

        private static async Task GenerateEmployeesAsync(int nrOfEmployees, List<Company> companies)
        {
            string[] positions = { "Developer", "Tester", "Manager" };
            var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
            {
                e.Name = f.Person.FullName;
                e.Age = (uint)f.Random.Int(min: 18, max: 67);
                e.Position = f.PickRandom(positions);
                e.Email = f.Person.Email;
                e.UserName = f.Person.UserName;
                e.Company = companies[f.Random.Int(0, companies.Count - 1)];
            });

            var users = faker.Generate(nrOfEmployees);

            var passWord = configuration["password"];

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, passWord);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                                
                if (user.Position == "manager")
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, employeeRole);
                }

            }
        }


        //private static ICollection<ApplicationUser>? GenerateEmployees(int nrOfEmployees)
        //{
        //string[] positions = { "Developer", "Tester", "Manager" };
        //var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
        //{
        //e.Name = f.Person.FullName;
        //e.Age = (uint)f.Random.Int(min: 18, max: 67);
        //e.Position = f.PickRandom(positions);
        //});

        //return faker.Generate(nrOfEmployees);
        //}
    }
}
