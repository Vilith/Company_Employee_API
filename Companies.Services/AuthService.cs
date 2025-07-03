using AutoMapper;
using Companies.API.Entities;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Companies.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        private ApplicationUser? user;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> CreateTokenAsync()
        {
            SigningCredentials signing = GetSigningCredentials();
            IEnumerable<Claim> claims = await GetClaimsAsync();
            JwtSecurityToken tokenOptions = GenerateTokenOptions(signing, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signing, IEnumerable<Claim> claims)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])),
                signingCredentials: signing
                );

            return tokenOptions;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(user));
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Age", user.Age.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secretKey = _config["secretkey"];
            ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }







        public async Task<IdentityResult> RegisterUserAsync(UserForRegistrationDTO registrationDTO)
        {
            ArgumentNullException.ThrowIfNull(registrationDTO);

            var roleExists = await _roleManager.RoleExistsAsync(registrationDTO.Role);
            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role doesn't exist" });
            }
            
            var user = _mapper.Map<ApplicationUser>(registrationDTO);
            
            var result = await _userManager.CreateAsync(user, registrationDTO.Password!);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registrationDTO.Role);
            }

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserForAuthDTO userForAuthDto)
        {
            if (userForAuthDto == null)
            {
                throw new ArgumentNullException(nameof(userForAuthDto));
            }

            user = await _userManager.FindByNameAsync(userForAuthDto.UserName);
            return user != null && await _userManager.CheckPasswordAsync(user, userForAuthDto.PassWord);
        }
    }
}
