using Api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController() 
        { 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthDTO dto)
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDTO dto)
        {
            return Ok();
        }
    }
}
