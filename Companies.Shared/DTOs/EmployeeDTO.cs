using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Shared.DTOs
{
    public record EmployeeDTO
    {
        public int Id { get; init; }               
        public uint Age { get; init; }        
        public string? Name { get; init; }        
        public string? Position { get; init; }        
    }
}
