using Companies.API.Services;
using Companies.Shared.DTOs;
using Companies.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Services.Contracts;

namespace Companies.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> companyService;
        private readonly Lazy<IEmployeeService> employeeService;
        private readonly Lazy<IAuthService> authService;
        public ICompanyService CompanyService => companyService.Value;
        public IEmployeeService EmployeeService => employeeService.Value;
        public IAuthService AuthService => authService.Value;

        //public ServiceManager(IUoW uow, IMapper mapper)
        //{           
        //    ArgumentNullException.ThrowIfNull(nameof(uow));

        //    companyService = new Lazy<ICompanyService>(() => new CompanyService(uow, mapper));
        //    employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(uow, mapper));
        //}
        public ServiceManager(Lazy<ICompanyService> companyservice, Lazy<IEmployeeService> employeeservice, Lazy<IAuthService> authservice)
        {
            companyService = companyservice;
            employeeService = employeeservice;
            authService = authservice;
        }
    }
}
