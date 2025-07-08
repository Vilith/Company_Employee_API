using AutoMapper;
using Domain.Models.Entities;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Companies.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        private ApplicationUser? _user;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //public async Task<string> CreateTokenAsync(bool expireTime)
        //{
        //    SigningCredentials signing = GetSigningCredentials();
        //    IEnumerable<Claim> claims = await GetClaimsAsync();
        //    JwtSecurityToken tokenOptions = GenerateTokenOptions(signing, claims);

        //    return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        //}


        public async Task<TokenDTO> CreateTokenAsync(bool expireTime)
        {
            ArgumentNullException.ThrowIfNull(nameof(_user));
            SigningCredentials signing = GetSigningCredentials();
            IEnumerable<Claim> claims = await GetClaimsAsync();
            JwtSecurityToken tokenOptions = GenerateTokenOptions(signing, claims);

            _user!.RefreshToken = GenerateRefreshToken();

            if (expireTime)
            {
                _user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7); 
            }

            var res = await _userManager.UpdateAsync(_user); // Validating res

            var accesstoken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDTO(accesstoken, _user.RefreshToken!);
        }

        private string? GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
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
            ArgumentNullException.ThrowIfNull(nameof(_user));
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim("Age", _user.Age.ToString()),
                new Claim(ClaimTypes.NameIdentifier, _user.Id)
            };

            var roles = await _userManager.GetRolesAsync(_user);
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

            _user = await _userManager.FindByNameAsync(userForAuthDto.UserName);
            return _user != null && await _userManager.CheckPasswordAsync(_user, userForAuthDto.PassWord);
        }

        public async Task<TokenDTO> RefreshTokenAsync(TokenDTO token)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token.AccessToken);

            ApplicationUser? user = await _userManager.FindByNameAsync(principal.Identity?.Name!);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpireTime <= DateTime.UtcNow)
            {
                throw new ArgumentException("The TokenDTO has some invalid values");
            }
                _user = user;

            return await CreateTokenAsync(expireTime: false);

            
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            ArgumentNullException.ThrowIfNull(nameof(jwtSettings));

            var secretKey = _config["secretkey"];
            ArgumentNullException.ThrowIfNull(nameof(secretKey));

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

                ValidateLifetime = false // Changes to false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, tokenValidationParams, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;

        }
    }
    
}
