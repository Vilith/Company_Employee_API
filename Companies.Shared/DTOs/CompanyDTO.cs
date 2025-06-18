using System.ComponentModel.DataAnnotations;

namespace Companies.API.DTOs
{
    public record CompanyDTO
    {
        public int Id { get; init; }               
        public string? Name { get; init; }
        public string? Address { get; init; }
        public string? Country { get; init; }
    }
}
