using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Companies.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(UserForRegistrationDTO registrationDTO)
        {
            var result = await _serviceManager.AuthService.RegisterUserAsync(registrationDTO);
            return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Authenticate(Companies.Shared.DTOs.UserForAuthDTO userForAuthDto)
        {
            if (!await _serviceManager.AuthService.ValidateUserAsync(userForAuthDto))
            {
                return Unauthorized();
            }

            //var token = new { Token = "Lattjo Lajban" };
            TokenDTO token = await _serviceManager.AuthService.CreateTokenAsync(expireTime: true);
            return Ok(token);

        }

    }
}
