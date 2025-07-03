using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(UserForRegistrationDTO registrationDTO);
    }
}