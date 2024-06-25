using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.Security.Service
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user, string password);
        Task<List<User>> GetAllUsersAsync();
    }
}
