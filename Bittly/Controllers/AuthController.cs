using Bittly.Configurations;
using Bittly.Dtos;
using Bittly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bittly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return Ok(new { Message = result.Value });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return Ok(result.Value);
        }

        // You can copy the HandleFailure method here or move it to a BaseController
    }
}