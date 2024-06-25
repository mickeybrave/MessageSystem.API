using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.BL.Auth
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<SignInResult> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<User> GetUserAsync(string username);
    }
}
