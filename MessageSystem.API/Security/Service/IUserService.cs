using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.Security.Service
{
    public interface IUserService
    {
        Task<IdentityUser> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(IdentityUser user);
        public List<IdentityUser> GetAllUsers();
    }
}
