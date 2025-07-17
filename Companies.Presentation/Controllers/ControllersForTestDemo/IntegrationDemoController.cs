using AutoMapper;
using Companies.API.Data;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Presentation.Controllers.ControllersForTestDemo
{
    [Route("api/demo")]
    [ApiController]
    public class IntegrationDemoController : ControllerBase
    {
        private readonly CompaniesContext _db;
        private readonly IMapper _mapper;

        public IntegrationDemoController(CompaniesContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok("Working");
        }

        [HttpGet("dto")]
        public ActionResult Index2()
        {
            var dto = new CompanyDTO { Name = "Working AB" };
            return Ok(dto);
        }

        [HttpGet("getone")]
        public async Task<ActionResult> GetOne()
        {
            var companies = (await _db.Companies.ToListAsync()).First();

            var compDto = _mapper.Map<CompanyDTO>(companies);

            return Ok(compDto);
        }

        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            var companies = await _db.Companies.ToListAsync();
            var compDtos = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            return Ok(compDtos);
        }

    }
}
