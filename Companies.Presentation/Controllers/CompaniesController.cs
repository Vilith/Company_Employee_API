using Microsoft.AspNetCore.Mvc;
using Companies.Shared.DTOs;
using Companies.Shared.Request;
using Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Domain.Models.Entities;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;



namespace Companies.Presentation.Controllers
{
    [Route("api/Companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {      
        private readonly IServiceManager _serviceManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(IServiceManager serviceManager, UserManager<ApplicationUser> userManager)
        {
            _serviceManager = serviceManager;
            _userManager = userManager;
        }                

        // GET: api/Companies
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompany([FromQuery] CompanyRequestParams requestParams)
        {
            //var companyDtos = await _serviceManager.CompanyService.GetCompaniesAsync(requestParams);
            var pagedResult = await _serviceManager.CompanyService.GetCompaniesAsync(requestParams);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.companyDtos);
        }             

        // GET: api/Companies/5
        [HttpGet("{id:int}")]
        //[Authorize]
        //[Authorize(Roles = "Admin")]
        //[AllowAnonymous]        
        //[Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<CompanyDTO>> GetCompany(int id)
        {            
            CompanyDTO dto = await _serviceManager.CompanyService.GetCompanyAsync(id);

            return Ok(dto);
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCompany(int id, UpdateCompanyDTO dto)
        //{
        //    if (id != dto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var existingCompany = await _uoW.CompanyRepository.GetCompanyAsync(id, trackChanges: true);
        //    if (existingCompany == null)
        //    {
        //        return NotFound("Company doesn't exist");
        //    }

        //    _mapper.Map(dto, existingCompany);
        //    await _uoW.CompleteAsync();

        //    return NoContent();
        //}

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> PostCompany(CreateCompanyDTO dto)
        {
            //var company = _mapper.Map<Company>(dto);

            //_uoW.CompanyRepository.Create(company);
            //await _uoW.CompleteAsync();

            //var createdCompany = _mapper.Map<CompanyDTO>(company);
            var createdCompany = await _serviceManager.CompanyService.PostAsync(dto);

            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        }

        //// DELETE: api/Companies/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCompany(int id)
        //{
        //    var company = await _uoW.CompanyRepository.GetCompanyAsync(id);
        //    if (company == null)
        //    {
        //        return NotFound("Company not found");
        //    }

        //    _uoW.CompanyRepository.Delete(company);
        //    await _uoW.CompleteAsync();

        //    return NoContent();

        //}
    }
}
