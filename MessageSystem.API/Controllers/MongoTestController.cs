using MessageSystem.API.Security.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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
        public IActionResult TestConnection()
        {
            var users = _userService.GetAllUsers();
            return Ok(new { count = users.Count });
        }
    }
}
