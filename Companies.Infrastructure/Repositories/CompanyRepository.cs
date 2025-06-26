using Companies.API.Data;
using Companies.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompaniesContext _context;
        public CompanyRepository(CompaniesContext context)
        {
            _context = context;
        }

        public async Task<Company?> GetCompanyAsync(int id)
        {
            return await _context.Companies.FindAsync(id);

        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees = false)
        {
            return includeEmployees ? await _context.Companies.Include(c => c.Employees).ToListAsync()
                                    : await _context.Companies.ToListAsync();
        }

        public void Add(Company company)
        {
            _context.Companies.Add(company);
        }

        public void Delete(Company company)
        {
            _context.Companies.Remove(company);
        }
    }
}
