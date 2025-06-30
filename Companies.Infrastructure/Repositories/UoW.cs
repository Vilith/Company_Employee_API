using Companies.API.Data;
using Companies.Infrastructure.Repositories;
using Domain.Contracts;

namespace Companies.API.Services
{
    public class UoW : IUoW
    {        
        private readonly CompaniesContext _context;

        private readonly Lazy<ICompanyRepository> companyRepository;
        private readonly Lazy<IEmployeeRepository> employeeRepository;

        public ICompanyRepository CompanyRepository => companyRepository.Value;
        public IEmployeeRepository EmployeeRepository => employeeRepository.Value;

        // Fler repos

        public UoW(CompaniesContext context)
        {
            _context = context;
            //CompanyRepository = new CompanyRepository(context);    
            //EmployeeRepository = new EmployeeRepository(context);
            companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(context));
            employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
        }               

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
