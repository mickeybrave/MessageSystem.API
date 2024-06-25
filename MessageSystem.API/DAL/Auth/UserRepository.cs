using MessageSystem.API.Security;
using MessageSystem.API.Security.Model;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace MessageSystem.API.DAL.Auth
{


    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(MongoDbContext context)
        {
            _usersCollection = context.Users ?? throw new ArgumentNullException(nameof(context.Users));
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            user.PasswordHash = HashPassword(password);
            await _usersCollection.InsertOneAsync(user);
            return IdentityResult.Success;
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            return await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var passwordHash = HashPassword(password);
            return user.PasswordHash == passwordHash;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task AddToRoleAsync(User user, Guid role)
        {
            if (!user.Roles.Contains(role))
            {
                user.Roles.Add(role);
                await UpdateUserAsync(user);
            }
        }

        public async Task<IList<Guid>> GetRolesAsync(User user)
        {
            return user.Roles;
        }

        public async Task<bool> IsInRoleAsync(User user, Guid role)
        {
            return user.Roles.Contains(role);
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        private string HashPassword(string password)
        {
            // Implement password hashing logic here
            // For demonstration, this is a simple placeholder
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task AddUserToRolesAsync(User user, IList<Guid> roles)
        {
            foreach (var role in roles)
            {
                if (!user.Roles.Contains(role))
                {
                    user.Roles.Add(role);
                }
            }
            await UpdateUserAsync(user);
        }
    }
}
