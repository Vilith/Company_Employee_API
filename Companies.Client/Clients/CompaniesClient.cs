using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Companies.Client.Clients
{
    public class CompaniesClient : ICompaniesClient
    {
        private readonly HttpClient _client;
        private const string _json = "application/json";

        public CompaniesClient(HttpClient client)
        {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7249/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_json));
            client.Timeout = new TimeSpan(0, 0, 5); // 0 hours, 0 minutes, 5 seconds

        }

        public async Task<T> GetAsync<T>(string path, string contentType = _json)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_json));

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return result!;
        }

    }
}
