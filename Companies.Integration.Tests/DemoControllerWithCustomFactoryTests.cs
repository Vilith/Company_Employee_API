using Companies.API;
using Companies.API.Data;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Http;
using Microsoft.Identity.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Integration.Tests
{
    public class DemoControllerWithCustomFactoryTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private HttpClient _httpClient;
        private CompaniesContext _context;

        public DemoControllerWithCustomFactoryTests(CustomWebApplicationFactory<Program> applicationFactory)
        {
            applicationFactory.ClientOptions.BaseAddress = new Uri("https://localhost:7249/api/demo/");
            _httpClient = applicationFactory.CreateClient();

            _context = applicationFactory.Context;
        }

        [Fact]
        public async Task Get_ShouldReturnCompany_FromInMemoryDataBase()
        {
            var dto = await _httpClient.GetFromJsonAsync<CompanyDTO>("getone");
            Assert.Equal("TestCompanyName", dto.Name);

        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCompanies_FromInMemoryDataBase()
        {
            var dtos = await _httpClient.GetFromJsonAsync<IEnumerable<CompanyDTO>>("getall");
            Assert.Equal(_context.Companies.Count(), dtos.Count());
            Assert.Equal(1, dtos.Count()); // Assuming there is 1 company in the in-memory database
        }
    }
}
