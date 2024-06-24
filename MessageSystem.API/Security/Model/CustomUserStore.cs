using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
namespace MessageSystem.API.Security.Model
{
    public class CustomUserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>
    {
        private readonly IMongoCollection<IdentityUser> _usersCollection;

        public CustomUserStore(IMongoCollection<IdentityUser> usersCollection)
        {
            _usersCollection = usersCollection;
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            _usersCollection.InsertOne(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            _usersCollection.DeleteOne(u => u.Id == user.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_usersCollection.Find(u => u.Id == userId).FirstOrDefault());
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_usersCollection.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefault());
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            _usersCollection.ReplaceOne(u => u.Id == user.Id, user);
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
            // No resource to dispose
        }
    }
}
