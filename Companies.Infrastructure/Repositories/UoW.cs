using Companies.API.Data;

namespace Companies.API.Services
{
    public class UoW : IUoW
    {        
        private readonly CompaniesContext _context;

        public ICompanyRepository CompanyRepository { get; }
        // Fler repos

        public UoW(CompaniesContext context)
        {
            _context = context;
            CompanyRepository = new CompanyRepository(context);
        }               

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
