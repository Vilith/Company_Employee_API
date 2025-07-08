using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<TokenDTO> CreateTokenAsync(bool expireTime);
        Task<TokenDTO> RefreshTokenAsync(TokenDTO token);
        Task<IdentityResult> RegisterUserAsync(UserForRegistrationDTO registrationDTO);
        Task<bool> ValidateUserAsync(UserForAuthDTO userForAuthDto);       
        
    }
}