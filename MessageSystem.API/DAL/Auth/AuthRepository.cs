using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.DAL.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(string username, string password)
        {
            var user = new IdentityUser { UserName = username };
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<SignInResult> LoginAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityUser> GetUserAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
    }

}
