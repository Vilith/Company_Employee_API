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
using System.Security.Claims;

namespace Companies.Presentation.Controllers.ControllersForTestDemo
{
    [Route("api/repo/{id}")]
    [ApiController]
    public class RepositoryController : ApiControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IUoW uoW;
        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepositoryController(IServiceManager seviceManager, IMapper mapper, UserManager<ApplicationUser> userManager) 
        {
            _serviceManager = seviceManager;
            //this.uoW = uoW;
            //_employeeRepository = employeeRepository;
            //_mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees(int id)
        {
                        
            var user = await _userManager.GetUserAsync(User);
            if (user is null) throw new ArgumentException(nameof(user));

            //var employees = await _seviceManager.EmployeeService.GetEmployeesAsync(id);

            var response = await _serviceManager.EmployeeService.GetEmployeesAsync(id);   
            
            //var dto = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            //return Ok(dto);
            return response.Success ? Ok(response.GetOkResult<IEnumerable<EmployeeDTO>>())
                                    : ProcessError(response);
        }

    }
}