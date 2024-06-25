using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.DAL.Auth
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task AddToRoleAsync(User user, Guid role);
        Task AddUserToRolesAsync(User user, IList<Guid> roles);
        Task<IList<Guid>> GetRolesAsync(User user);
        Task<bool> IsInRoleAsync(User user, Guid role);
        Task<IList<User>> GetAllUsersAsync();
    }

}
