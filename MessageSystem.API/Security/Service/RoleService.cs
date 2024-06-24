using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace MessageSystem.API.Security.Service
{
    public class RoleService : IRoleService
    {
        private readonly MongoDbContext _context;

        public RoleService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateRoleAsync(IdentityRole role)
        {
            await _context.Roles.InsertOneAsync(role);
        }
    }
}
