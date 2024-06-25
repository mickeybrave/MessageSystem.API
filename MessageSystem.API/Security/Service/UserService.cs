using MessageSystem.API.DAL.Auth;
using MessageSystem.API.Security.Model;
using MongoDB.Driver;

namespace MessageSystem.API.Security.Service
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.FindByUsernameAsync(username);
        }

        public async Task CreateUserAsync(User user, string password)
        {
            await _userRepository.CreateUserAsync(user, password);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return (await _userRepository.GetAllUsersAsync()).ToList();
        }
    }

}
