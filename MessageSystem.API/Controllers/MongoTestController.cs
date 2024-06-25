using MessageSystem.API.Security.Service;
using Microsoft.AspNetCore.Mvc;

namespace MessageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MongoTestController : ControllerBase
    {
        private readonly IUserService _userService;

        public MongoTestController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("")]
        public async Task<IActionResult> TestConnection()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new { count = users.Count });
        }
    }
}
