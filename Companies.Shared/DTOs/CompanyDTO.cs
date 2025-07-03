using Companies.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Companies.Shared.DTOs
{
    public record CompanyDTO
    {
        public int Id { get; init; }               
        public string? Name { get; init; }
        public string? Address { get; init; }
        //public string? Country { get; init; }

        public IEnumerable<EmployeeDTO> Employees { get; init; }

    }
}
