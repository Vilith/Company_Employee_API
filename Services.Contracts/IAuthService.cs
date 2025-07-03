
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync();
        Task<IdentityResult> RegisterUserAsync(UserForRegistrationDTO registrationDTO);
        Task<bool> ValidateUserAsync(UserForAuthDTO userForAuthDto);       
        
    }
}