using Companies.Shared.DTOs;
using Companies.Shared.Request;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Companies.Presentation.Controllers
{
    [Route("api/simple")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        public SimpleController()
        {
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
    }
}