using Domain.Models.Entities;
using Companies.Shared.Request;


namespace Domain.Contracts
{
    public interface ICompanyRepository
    {
        Task<PagedList<Company>> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false);
        Task<Company?> GetCompanyAsync(int id, bool trackChanges = false);

        void Create(Company company);
        void Delete(Company company);        

        Task<bool> CompanyExistAsync(int id);
    }
}