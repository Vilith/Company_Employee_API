using AutoMapper;
using Companies.API.Entities;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
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
        
    }
}
