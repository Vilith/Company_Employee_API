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

        public ICompanyService CompanyService => companyService.Value;
        public IEmployeeService EmployeeService => employeeService.Value;

        //public ServiceManager(IUoW uow, IMapper mapper)
        //{           
        //    ArgumentNullException.ThrowIfNull(nameof(uow));

        //    companyService = new Lazy<ICompanyService>(() => new CompanyService(uow, mapper));
        //    employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(uow, mapper));
        //}
        public ServiceManager(Lazy<ICompanyService> companyservice, Lazy<IEmployeeService> employeeservice)
        {
            companyService = companyservice;
            employeeService = employeeservice;
        }
    }
}
