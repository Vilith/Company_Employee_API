using AutoMapper;
using Companies.API.DTOs;
using Companies.API.Entities;
using Companies.API.Services;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public CompanyService(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }        

        public async Task<IEnumerable<CompanyDTO>> GetCompaniesAsync(bool includeEmployees, bool trackChanges = false)
        {
            return _mapper.Map<IEnumerable<CompanyDTO>>(await _uow.CompanyRepository.GetCompaniesAsync(includeEmployees, trackChanges));
        }

        public async Task<CompanyDTO> GetCompanyAsync(int id, bool trackChanges = false)
        {
            Company? company = await _uow.CompanyRepository.GetCompanyAsync(id);
            return _mapper.Map<CompanyDTO>(company);
        }
    }
}
