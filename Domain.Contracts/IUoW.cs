using Domain.Contracts;

namespace Companies.API.Services
{
    public interface IUoW
    {
        ICompanyRepository CompanyRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }

        Task CompleteAsync();
    }
}
