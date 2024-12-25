using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Shared.DataTransferObjects;
using Service.Contracts;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IServiceManager _serviceManager;

        public AuthController(ITokenService tokenService, IServiceManager serviceManager)
        {
            _tokenService = tokenService;
            _serviceManager = serviceManager;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Invalid client request");

            var user = await _serviceManager.UserService.GetUserByUsernameAsync(loginDto.Username, trackChanges: false);

            if (user == null || user.PasswordHash != loginDto.Password) 
                return Unauthorized("Invalid username or password.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("id", user.Id.ToString()), 
                new Claim(ClaimTypes.Role, user.Role) 
            };

            var token = _tokenService.CreateToken(user.Username, claims);

            return Ok(new { Token = token });
        }

    }

}
