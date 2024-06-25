using MessageSystem.API.BL.Auth;
using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Mvc;

namespace MessageSystem.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result.Succeeded)
                return Ok("User registered successfully.");
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _authService.LoginAsync(model);
            if (result.Succeeded)
                return Ok("Login successful.");
            return Unauthorized("Invalid login attempt.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok("Logout successful.");
        }
    }
}
