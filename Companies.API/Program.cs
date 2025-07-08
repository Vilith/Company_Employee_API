using Microsoft.EntityFrameworkCore;
using Companies.API.Data;
using Companies.API.Extensions;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using Domain.Models.Entities;

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

            builder.Services.AddControllers(configure =>
            {
                //configure.ReturnHttpNotAcceptable = true;
                //// Everyone has to be Admins
                //var policy = new AuthorizationPolicyBuilder()
                //            .RequireAuthenticatedUser()
                //            .RequireRole("Admin")
                //            .Build();
                //configure.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddNewtonsoftJson()
            .AddApplicationPart(typeof(AssemblyReference).Assembly);
            //.AddXmlDataContractSerializerFormatters();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.ConfigureOpenApi();

            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            //builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            //builder.Services.AddScoped<IUoW, UoW>();
            //builder.Services.AddScoped<IServiceManager, ServiceManager>();



            builder.Services.ConfigureServiceLayerServices(); // Use the extension method to configure service layer services
            builder.Services.ConfigureRepositories(); // Use the extension method to configure repositories


            builder.Services.ConfigureJwt(builder.Configuration);


            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //{
            //    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            //    ArgumentNullException.ThrowIfNull(nameof(jwtSettings));

            //    var secretKey = builder.Configuration["secretkey"];
            //    ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = jwtSettings["Issuer"],

            //        ValidateAudience = true,
            //        ValidAudience = jwtSettings["Audience"],

            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

            //        ValidateLifetime = true,
            //    };
            //});





            builder.Services.AddIdentityCore<ApplicationUser>(opt =>
            {
                // Configure password requirements
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                // Configure user settings
                opt.User.RequireUniqueEmail = true;
            }
            ).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CompaniesContext>()
            .AddDefaultTokenProviders();

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy", policy =>
            //    policy.RequireRole("Admin")
            //          .RequireClaim(ClaimTypes.NameIdentifier)
            //          .RequireClaim(ClaimTypes.Role));

            //    options.AddPolicy("EmployeePolicy", policy =>
            //    policy.RequireRole("Employee"));
            //});



            // Add CORS policy to allow all origins, headers, and methods
            //builder.Services.AddCors(builder => 
            //{
            //    builder.AddPolicy("AllowAll", p =>
            //    p.AllowAnyOrigin()
            //    .AllowAnyHeader()
            //    .AllowAnyMethod());
            //});

            builder.Services.ConfigureCors(); // Use the extension method to configure CORS


            var app = builder.Build();


            app.ConfigureExceptionHandler();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync(); // Seed the database with initial data
            }

            app.UseHttpsRedirection();

            // Use the CORS policy defined above
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
