﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Companies.API.Data;
using Companies.API.Entities;
using Companies.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Companies.API.Controllers
{
    [Route("api/Companies/{companyID}/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly CompaniesContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(CompaniesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Companies/2/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees(int companyID)
        {
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == companyID);
            if (!companyExists)
            {
                return NotFound($"Company with ID {companyID} not found.");
            }

            var employees = await _context.Employees
                .Where(e => e.CompanyId.Equals(companyID))
                .ToListAsync();
            var employeesDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            return Ok(employeesDTOs);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, int companyId)
        {
            var companyExists = await _context.Companies.AnyAsync(c => c.Id.Equals(companyId));
            if (!companyExists)
            {
                return NotFound($"Company with ID {companyId} not found.");
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id.Equals(id) && e.CompanyId.Equals(companyId));
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchEmployee(int id, int companyId, JsonPatchDocument<UpdateEmployeeDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest("No patchdocument");
            
            var companyExists = await _context.Companies.AnyAsync(c => c.Id.Equals(companyId));
            if (!companyExists) 
                return NotFound($"Company with ID {companyId} not found.");

            var employeeToPatch = await _context.Employees.FirstOrDefaultAsync(e => e.Id.Equals(id) && e.CompanyId.Equals(companyId));
            if (employeeToPatch == null) 
                return NotFound("Employee not found");

            var dto = _mapper.Map<UpdateEmployeeDTO>(employeeToPatch);
            patchDocument.ApplyTo(dto, ModelState); // This is where the dto is patched with the patchDocument
            TryValidateModel(dto);

            if (!ModelState.IsValid) 
                return UnprocessableEntity(ModelState);

            _mapper.Map(dto, employeeToPatch);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
