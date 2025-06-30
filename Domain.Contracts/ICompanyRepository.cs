using Companies.API.Entities;


namespace Companies.API.Services
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees = false, bool trackChanges = false);
        Task<Company?> GetCompanyAsync(int id, bool trackChanges = false);
        void Create(Company company);
        void Delete(Company company);
        void Update(Company company);

        Task<bool> CompanyExistAsync(int id);
    }
}