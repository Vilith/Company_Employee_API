using AutoMapper;
using Companies.Shared.Request;
using Companies.Shared.DTOs;
using Companies.Services;
using Domain.Models.Exceptions;
using Domain.Models.Entities;
using Domain.Contracts;
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

        public async Task<(IEnumerable<CompanyDTO> companyDtos, MetaData metaData)> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false)
        {
            var pagedList = await _uow.CompanyRepository.GetCompaniesAsync(requestParams, trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDTO>>(pagedList.Items);

            return (companiesDto, pagedList.MetaData);


            //return _mapper.Map<IEnumerable<CompanyDTO>>(await _uow.CompanyRepository.GetCompaniesAsync(requestParams, trackChanges));
        }

        public async Task<CompanyDTO> GetCompanyAsync(int id, bool trackChanges = false)
        {
            Company? company = await _uow.CompanyRepository.GetCompanyAsync(id);

            if (company == null)
            {
                throw new CompanyNotFoundException(id);
            }

            return _mapper.Map<CompanyDTO>(company);
        }

        public async Task<CompanyDTO> PostAsync(CreateCompanyDTO dto)
        {
            var company = _mapper.Map<Company>(dto);
            _uow.CompanyRepository.Create(company);
            await _uow.CompleteAsync();
            return _mapper.Map<CompanyDTO>(company);
        }
    }
}


/* Vincents rader:
 * vincent christoffer 
 * fgtrgtrtrttrrteeecvbfryuijodswertyyujhbvccdfrrsdfgfrttgnngtyhjiu8745 6dfghhjhjhgrddgvvbnmjkl iophhsdertyghjyujjhjhtyujferswvbtrrrdfggrtgrtttvghhy6yhybncnmjgtlumnbvftyuutrttrrfgfvg
 * Vincents mamma heter Johanna
 * Vincents pappa heter Christoffer  
 */
 