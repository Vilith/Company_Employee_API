namespace Companies.API.Services
{
    public interface IUoW
    {
        ICompanyRepository CompanyRepository { get; }

        Task CompleteAsync();
    }
}
