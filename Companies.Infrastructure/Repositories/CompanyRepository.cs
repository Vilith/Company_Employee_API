using Companies.API.Data;
using Domain.Models.Entities;
using Companies.Shared.Request;
using Microsoft.EntityFrameworkCore;
using Domain.Contracts;

namespace Companies.Infrastructure.Repositories
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

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false)
        {
            //return includeEmployees ? await _context.Companies.Include(c => c.Employees).ToListAsync()
            //: await _context.Companies.ToListAsync();
            //return includeEmployees ? await FindAll(trackChanges).Include(c => c.Employees).ToListAsync()
            //: await FindAll(trackChanges).ToListAsync();

            var companies = requestParams.IncludeEmployees ? FindAll(trackChanges).Include(c => c.Employees)
                                                           : FindAll(trackChanges);

            return await PagedList<Company>.CreateAsync(companies, requestParams.PageNumber, requestParams.PageSize);

        }

        public async Task<bool> CompanyExistAsync(int id)
        {
            return await Context.Companies.AnyAsync(c => c.Id.Equals(id));
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
