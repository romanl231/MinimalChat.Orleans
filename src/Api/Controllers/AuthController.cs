using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJWTService _jwtService;

        public AuthController(IAuthService authService, IJWTService jWTService) 
        { 
            _authService = authService;
            _jwtService = jWTService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto) => 
            await _authService.RegisterAsync(dto) ? 
                Ok(dto) : BadRequest();

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var userId = await _authService.LoginAsync(dto);

            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var token = _jwtService.GenerateJWTAsync(userId);
            return HandleLoginResult(token);
        }

        private IActionResult HandleLoginResult(string token)
        {
            Response.Cookies.Append("Bearer", token);
            return Ok();
        }
    }
}
