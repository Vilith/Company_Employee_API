using AutoMapper;
using Companies.API.Services;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public EmployeeService(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
    }
}
