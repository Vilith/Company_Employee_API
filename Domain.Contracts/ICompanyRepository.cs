using Companies.API.Entities;


namespace Companies.API.Services
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees = false);
        Task<Company?> GetCompanyAsync(int id);
        void Add(Company company);
        void Delete(Company company);
    }
}