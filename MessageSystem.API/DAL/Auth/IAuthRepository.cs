using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.DAL.Auth
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterAsync(string username, string password);
        Task<SignInResult> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<IdentityUser> GetUserAsync(string username);
    }

}
