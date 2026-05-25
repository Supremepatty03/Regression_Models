using ColorRegressionApp.DTO;
using ColorRegressionApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColorRegressionApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result)
                return BadRequest("Username already exists.");

            return Ok("Registration successful.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null)
                return Unauthorized("Invalid username or password.");

            return Ok(new { Token = token });
        }
    }
}