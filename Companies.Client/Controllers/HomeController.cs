using Companies.Client.Clients;
using Companies.Client.Models;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace Companies.Client.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient _httpClient;
        private const string _json = "application/json";
        private readonly CompaniesClient _companiesClient;

        public HomeController(IHttpClientFactory _httpClientFactory, CompaniesClient companiesClient)
        {
            // _httpClient = new HttpClient();
            //_httpClient = _httpClientFactory.CreateClient();

            //_httpClient.BaseAddress = new Uri("https://localhost:7249/");

            _httpClient = _httpClientFactory.CreateClient("CompaniesClient");
            _companiesClient = companiesClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await SimpleGetAsync();
            var result2 = await SimpleGetAsync2();

            var result3 = await GetWithRequestMessageAsync();
            var result4 = await PostWithRequestMessageAsync();

            await PatchWithRequestMessageAsync();

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

            return await _companiesClient.GetAsync<IEnumerable<CompanyDTO>>("api/companies");

            // This is now located in CompaniesClient.cs
            //var request = new HttpRequestMessage(HttpMethod.Get, "api/companies");
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_json));

            //var response = await _httpClient.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            //var res = await response.Content.ReadAsStringAsync();
            //var companies = JsonSerializer.Deserialize<IEnumerable<CompanyDTO>>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //return companies!;
        }

        private async Task<CompanyDTO> PostWithRequestMessageAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/companies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_json));

            var companyToCreate = new CreateCompanyDTO
            {
                Name = "Bluff & Båg AB",
                Address = "Storgatan 5",
                Country = "Sweden",
                Employees = null
            };

            var jsonCompany = JsonSerializer.Serialize(companyToCreate);

            request.Content = new StringContent(jsonCompany);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(_json);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadAsStringAsync();
            var companyDto = JsonSerializer.Deserialize<CompanyDTO>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var location = response.Headers.Location;

            return companyDto!;
        }

        private async Task PatchWithRequestMessageAsync()
        {
            var patchDoc = new JsonPatchDocument<UpdateEmployeeDTO>();

            patchDoc.Replace(e => e.Age, (uint)95);
            patchDoc.Replace(e => e.Name, "Ben Dover");

            var serializedPatchDoc = Newtonsoft.Json.JsonConvert.SerializeObject(patchDoc);

            var request = new HttpRequestMessage(HttpMethod.Patch, "api/companies/1/employees/0c24ffb6-1c05-4fcd-85f8-c4b86b76e840");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_json));

            request.Content = new StringContent(serializedPatchDoc);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(_json);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            

        }
    }
}
