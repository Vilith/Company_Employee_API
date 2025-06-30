using Companies.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetCompaniesAsync(bool includeEmployees, bool trackChanges = false);
        Task<CompanyDTO> GetCompanyAsync(int id, bool trackChanges = false);
    }
}
