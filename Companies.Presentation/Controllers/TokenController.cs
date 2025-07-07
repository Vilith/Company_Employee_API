using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Companies.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TokenController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken(TokenDTO token)
        {
            TokenDTO tokenDto = await _authService.RefreshTokenAsync(token);
            return Ok(tokenDto);
        }
    }
}
