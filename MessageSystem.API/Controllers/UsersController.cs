using MessageSystem.API.Security.Model;
using MessageSystem.API.Security.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MessageSystem.API.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    //public class UsersController : ControllerBase
    //{
    //    private readonly IUserService _userService;

    //    public UsersController(IUserService userService)
    //    {
    //        _userService = userService;
    //    }

    //    [HttpGet("{username}")]
    //    public async Task<IActionResult> GetUserByUsername(string username)
    //    {
    //        var user = await _userService.GetUserByUsernameAsync(username);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(user);
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> CreateUser(LoginModel user)
    //    {
    //        await _userService.CreateUserAsync(user);
    //        return CreatedAtAction(nameof(GetUserByUsername), new { username = user.Username }, user);
    //    }
    //}
}
