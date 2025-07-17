using Companies.API;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Companies.Integration.Tests
{
    public class DemoControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _httpClient;

        public DemoControllerTest(WebApplicationFactory<Program> applicationFactory)
        {
            applicationFactory.ClientOptions.BaseAddress = new Uri("https://localhost:7249/api/demo/");
            _httpClient = applicationFactory.CreateClient();

        }

        [Fact]
        public async Task ShouldReturnOk()
        {
            var response = await _httpClient.GetAsync("");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnExpectedMessage()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Working", content);
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task ShouldReturnExpectedMediaType()
        {
            var response = await _httpClient.GetAsync("dto");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<CompanyDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("Working AB", dto.Name);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Index3_ShouldReturnExpectedMessage_WithStream()
        {
            var response = await _httpClient.GetStreamAsync("dto");

            var dto = await JsonSerializer.DeserializeAsync<CompanyDTO>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("Working AB", dto.Name);

        }

        [Fact]
        public async Task Index4_ShouldReturnExpectedMessageSimplified()
        {
            var dto = await _httpClient.GetFromJsonAsync<CompanyDTO>("dto");
            Assert.Equal("Working AB", dto.Name);

        }
    }    
}