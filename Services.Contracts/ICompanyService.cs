using Companies.Shared.DTOs;
using Companies.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ICompanyService
    {
        Task<(IEnumerable<CompanyDTO> companyDtos, MetaData metaData)> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false);
        Task<CompanyDTO> GetCompanyAsync(int id, bool trackChanges = false);
    }
}
