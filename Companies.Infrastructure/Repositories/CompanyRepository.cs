using Companies.API.Data;
using Companies.API.Entities;
using Companies.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Services
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        //private readonly CompaniesContext _context;

        public CompanyRepository(CompaniesContext context) : base(context)
        {
            //_context = context;
        }

        public async Task<Company?> GetCompanyAsync(int id, bool trackChanges = false)
        {
            //return await _context.Companies.FindAsync(id);
            return await FindByCondition(c => c.Id.Equals(id), trackChanges).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees = false, bool trackChanges = false)
        {
            //return includeEmployees ? await _context.Companies.Include(c => c.Employees).ToListAsync()
            //: await _context.Companies.ToListAsync();
            return includeEmployees ? await FindAll(trackChanges).Include(c => c.Employees).ToListAsync()
                                    : await FindAll(trackChanges).ToListAsync();

        }

        //public void Create(Company company)
        //{
        //    _context.Companies.Add(company);
        //}

        //public void Delete(Company company)
        //{
        //    _context.Companies.Remove(company);
        //}                
    }
}
