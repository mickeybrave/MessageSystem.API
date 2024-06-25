using MessageSystem.API.DAL.Auth;
using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.BL.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(IUserRepository userRepository, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new User { Username = model.Username, Email = model.Email, Roles = model.Roles.Select(s => Guid.Parse(s)).ToList() };
            var result = await _userRepository.CreateUserAsync(user, HashPassword(model.Password));
            if (result.Succeeded)
            {
                await _userRepository.AddUserToRolesAsync(user, user.Roles);
            }
            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            var user = await _userRepository.FindByUsernameAsync(model.Username);
            if (user == null)
                return SignInResult.Failed;

            var passwordCorrect = await _userRepository.CheckPasswordAsync(user, model.Password);
            if (!passwordCorrect)
                return SignInResult.Failed;

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _userRepository.FindByUsernameAsync(username);
        }

        private string HashPassword(string password)
        {
            // Implement a proper password hashing method
            return password; // Placeholder
        }
    }




}
