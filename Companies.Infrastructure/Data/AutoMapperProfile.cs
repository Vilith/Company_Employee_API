using AutoMapper;
using Companies.API.Entities;
using Companies.API.DTOs;
using Companies.Shared.DTOs;

namespace Companies.API.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom
                (src => $"{src.Address}{(string.IsNullOrEmpty(src.Country) ? string.Empty : ", ")}{src.Country}"));

            CreateMap<ApplicationUser, EmployeeDTO>().ReverseMap();

            CreateMap<CreateCompanyDTO, Company>();               
            CreateMap<UpdateCompanyDTO, Company>();
            CreateMap<ApplicationUser, UpdateEmployeeDTO>().ReverseMap();
        }
    }
}
