using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Companies.API.Data;
using System.Threading.Tasks;
using Companies.API.Services;

namespace Companies.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<CompaniesContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CompaniesContext") ?? throw new InvalidOperationException("Connection string 'CompaniesContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson();
            //.AddXmlDataContractSerializerFormatters();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            //builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IUoW, UoW>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync(); // Seed the database with initial data
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
