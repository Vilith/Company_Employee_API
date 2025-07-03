using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
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

    }
}
