using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Contracts;
using Domain.Models.Responses;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public EmployeeService(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> GetEmployeesAsync(int companyID)
        {
            var companyExist = await _uow.CompanyRepository.CompanyExistAsync(companyID);

            if (!companyExist)
            {
                return new CompanyNotFoundResponse(companyID);
            }

            var employees = await _uow.EmployeeRepository.GetEmployeesAsync(companyID);
            
            var employeesDtos = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            return new ApiOkResponse<IEnumerable<EmployeeDTO>>(employeesDtos);
        }
    }
}
