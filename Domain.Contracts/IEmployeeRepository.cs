using Companies.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IEmployeeRepository
    {
        void Create(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);

        Task<IEnumerable<Employee>> GetEmployeesAsync(int companyId, bool trackChanges = false);
        Task<Employee?> GetEmployeeAsync(int companyId, int employeeId, bool trackChanges = false);
    }
}
