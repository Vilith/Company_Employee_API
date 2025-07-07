using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        public string Title { get; set; }

        protected NotFoundException(string message, string title = "Not Found") : base(message) 
        {
            Title = title;
            
        }
    }

    public class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(int id) : base($"The Company with id: {id} was not found")
        {
            
        }
    }

    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(int id) : base($"The Employee with id: {id} was not found")
        {
            
        }
    }
}
