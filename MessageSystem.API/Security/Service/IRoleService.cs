using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;

namespace MessageSystem.API.Security.Service
{
    public interface IRoleService
    {
        Task CreateRoleAsync(IdentityRole role);
    }
}
