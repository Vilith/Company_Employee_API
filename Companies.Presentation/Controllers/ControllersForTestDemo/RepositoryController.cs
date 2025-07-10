using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Domain.Models.Entities;
using Companies.Shared.Request;
using System.Net.Http.Headers;
using System.Text.Json;
using Domain.Contracts;
using AutoMapper;
using Companies.Shared.DTOs;

namespace Companies.Presentation.Controllers.ControllersForTestDemo
{
    [Route("api/repo/{id}")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public RepositoryController(IEmployeeRepository employeeRepository, IMapper mapper) 
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees(int id)
        {
            var employees = await _employeeRepository.GetEmployeesAsync(id);
            var dto = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            return Ok(dto);
        }

    }
}
