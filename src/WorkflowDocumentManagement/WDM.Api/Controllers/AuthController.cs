using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WDM.Domain.Dtos;
using WDM.Domain.Services;

namespace WDM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.AuthenticateAsync(request);

            if (!result.IsSuccess)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                token = result.Token,
                admin = new
                {
                    result.User?.Id,
                    result.User?.Email,
                    result.User?.UserName,
                    result.User?.AccessLevel
                }
            });
        }
    }
}
