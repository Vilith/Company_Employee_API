using Microsoft.AspNetCore.Mvc;
using Companies.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Domain.Contracts;
using Domain.Models.Responses;
using Services.Contracts;

namespace Companies.Presentation.Controllers
{
    [Route("api/Companies/{companyID}/Employees")]
    [ApiController]
    public class EmployeesController : ApiControllerBase
    {        
        private readonly IMapper _mapper;
        private readonly IUoW _uow;
        private readonly IServiceManager _serviceManager;

        public EmployeesController(IMapper mapper, IUoW uow, IServiceManager serviceManager)
        {            
            _mapper = mapper;
            _uow = uow;
            _serviceManager = serviceManager;
        }

        // GET: api/Companies/2/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees(int companyID)
        {
            ApiBaseResponse response = await _serviceManager.EmployeeService.GetEmployeesAsync(companyID);

            return response.Success ? Ok(response.GetOkResult<IEnumerable<EmployeeDTO>>()) : ProcessError(response);  



            //var companyExists = await _context.Companies.AnyAsync(c => c.Id == companyID);
            //var companyExists = await _uow.CompanyRepository.CompanyExistAsync(companyID);
            //if (!companyExists)
            //{
            //    return NotFound($"Company with ID {companyID} not found.");
            //}

            ////var employees = await _context.Employees
            //    //.Where(e => e.CompanyId.Equals(companyID))
            //    //.ToListAsync();

            //var employees = await _uow.EmployeeRepository.GetEmployeesAsync(companyID);

            //var employeesDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            //return Ok(employeesDTOs);
        }

        //// GET: api/Employees/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Employee>> GetEmployee(int id)
        //{
        //    var employee = await _context.Employees.FindAsync(id);

        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return employee;
        //}

        //// PUT: api/Employees/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEmployee(int id, Employee employee)
        //{
        //    if (id != employee.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(employee).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EmployeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Employees
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        //{
        //    _context.Employees.Add(employee);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        //}

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id, int companyId)
        {            
            var companyExists = await _uow.CompanyRepository.CompanyExistAsync(companyId);
            if (!companyExists)
            {
                return NotFound($"Company with ID {companyId} not found.");
            }
                     
            var employee = await _uow.EmployeeRepository.GetEmployeeAsync(companyId, id);


            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _uow.EmployeeRepository.Delete(employee);
            await _uow.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchEmployee(int companyId, string id, JsonPatchDocument<UpdateEmployeeDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest("No patchdocument");
                        
            var companyExists = await _uow.CompanyRepository.CompanyExistAsync(companyId);  
            if (!companyExists) 
                return NotFound($"Company with ID {companyId} not found.");
                        
            var employeeToPatch = await _uow.EmployeeRepository.GetEmployeeAsync(companyId, id, trackChanges: true);
            if (employeeToPatch == null) 
                return NotFound("Employee not found");

            var dto = _mapper.Map<UpdateEmployeeDTO>(employeeToPatch);
            patchDocument.ApplyTo(dto);
            TryValidateModel(dto);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(dto, employeeToPatch);            
            await _uow.CompleteAsync();

            return NoContent();
        }              
    }
}
