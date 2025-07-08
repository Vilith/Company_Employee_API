using Domain.Contracts;

namespace Domain.Contracts
{
    public interface IUoW
    {
        ICompanyRepository CompanyRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }

        Task CompleteAsync();
    }
}
