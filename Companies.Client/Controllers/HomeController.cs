using Companies.Client.Models;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Companies.Client.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient _httpClient;
        private const string _json = "application/json";

        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7249/");
        }

        public async Task<IActionResult> Index()
        {
            var result = await SimpleGetAsync();
            var result2 = await SimpleGetAsync2();

            var result3 = await GetWithRequestMessageAsync();


            return View();
        }

        private async Task<IEnumerable<CompanyDTO>> SimpleGetAsync()
        {
            var response = await _httpClient.GetAsync("api/companies");
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadAsStringAsync();

            var companies = JsonSerializer.Deserialize<IEnumerable<CompanyDTO>>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return companies!;

        }

        private async Task<IEnumerable<CompanyDTO>> SimpleGetAsync2()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CompanyDTO>>("api/companies", new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private async Task<IEnumerable<CompanyDTO>> GetWithRequestMessageAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/companies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_json));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadAsStringAsync();
            var companies = JsonSerializer.Deserialize<IEnumerable<CompanyDTO>>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return companies!;
        }



    }
}
