using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace MessageSystem.API.Security.Service
{

    public class UserService : IUserService
    {
        private readonly MongoDbContext _context;

        public UserService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IdentityUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Find(u => u.UserName == username).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(IdentityUser user)
        {
            await _context.Users.InsertOneAsync(user);
        }

        public List<IdentityUser> GetAllUsers()
        {
            return _context.Users.Find(_ => true).ToList();
        }
    }
}
