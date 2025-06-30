using Companies.API.Data;
using Companies.Infrastructure.Repositories;
using Domain.Contracts;

namespace Companies.API.Services
{
    public class UoW : IUoW
    {        
        private readonly CompaniesContext _context;

        public ICompanyRepository CompanyRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }

        // Fler repos

        public UoW(CompaniesContext context)
        {
            _context = context;
            CompanyRepository = new CompanyRepository(context);    
            EmployeeRepository = new EmployeeRepository(context);
        }               

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
