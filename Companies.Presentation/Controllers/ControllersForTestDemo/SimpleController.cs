using AutoMapper;
using Companies.API.Data;
using Companies.Shared.DTOs;
using Companies.Shared.Request;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Companies.Presentation.Controllers
{
    [Route("api/simple")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        private readonly CompaniesContext db;
        private readonly IMapper mapper;

        public SimpleController(CompaniesContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompany()
        {
            //return Ok("Hej från controllern");

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return Ok("Is Authenticated");
            }
            else
            {
                return BadRequest("Is Not Authenticated");
            }
        }

        [HttpGet("uniqueroute")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompany2()
        {
            var companies = await db.Companies.ToListAsync();
            var compdtos = mapper.Map<IEnumerable<CompanyDTO>>(companies);
            return Ok(compdtos);
        }
    }
}