﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Companies.API.Data;
using Companies.API.Entities;
using Companies.API.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Companies.Shared.DTOs;
using Companies.API.Services;
using Companies.Infrastructure;

namespace Companies.API.Controllers
{
    [Route("api/Companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        //private readonly CompaniesContext _context;
        private readonly IMapper _mapper;
        private readonly IUoW _uoW;
        //private readonly ICompanyRepository _companyRepo;

        public CompaniesController(IMapper mapper, IUoW uoW)
        {
            //_context = context;
            _mapper = mapper;
            _uoW = uoW;
            //_companyRepo = companyRepo;
        }                

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompany(bool includeEmployees)
        {
            //return await _context.Company.ToListAsync();
            //return await _context.Company.Include(c => c.Employees).ToListAsync();
            //var companies = _context.Companies.Select(c => new CompanyDTO
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    Address = c.Address,
            //    Country = c.Country
            //});

            //var companies = await _context.Companies.ProjectTo<CompanyDTO>(_mapper.ConfigurationProvider).ToListAsync();
            //var companies = includeEmployees ? _mapper.Map<IEnumerable<CompanyDTO>>(await _context.Companies.Include(c => c.Employees).ToListAsync())
                                             //: _mapper.Map<IEnumerable<CompanyDTO>>(await _context.Companies.ToListAsync());

            var companies = includeEmployees ? _mapper.Map<IEnumerable<CompanyDTO>>(await _uoW.CompanyRepository.GetCompaniesAsync(true))
                                             : _mapper.Map<IEnumerable<CompanyDTO>>(await _uoW.CompanyRepository.GetCompaniesAsync());

            return Ok(companies);
        }             

        // GET: api/Companies/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyDTO>> GetCompany(int id)
        {
            var company = await _uoW.CompanyRepository.GetCompanyAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            //var dto = new CompanyDTO
            //{
            //    Id = company.Id,
            //    Name = company.Name,
            //    Address = company.Address,
            //    Country = company.Country
            //};

            var dto = _mapper.Map<CompanyDTO>(company);

            return dto;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, UpdateCompanyDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }
            
            var existingCompany = await _uoW.CompanyRepository.GetCompanyAsync(id, trackChanges: true);
            if (existingCompany == null)
            {
                return NotFound("Company doesn't exist");
            }

            _mapper.Map(dto, existingCompany);
            await _uoW.CompleteAsync();

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> PostCompany(CreateCompanyDTO dto)
        {
            var company = _mapper.Map<Company>(dto);
                        
            _uoW.CompanyRepository.Create(company);
            await _uoW.CompleteAsync();

            var createdCompany = _mapper.Map<CompanyDTO>(company);

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, createdCompany);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _uoW.CompanyRepository.GetCompanyAsync(id);
            if (company == null)
            {
                return NotFound("Company not found");
            }

            _uoW.CompanyRepository.Delete(company);
            await _uoW.CompleteAsync();

            return NoContent();

        }
    }
}
