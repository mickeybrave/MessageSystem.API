using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace MessageSystem.API.Security.Model
{
    public class CustomRoleStore : IRoleStore<IdentityRole>
    {
        private readonly IMongoCollection<IdentityRole> _rolesCollection;

        public CustomRoleStore(IMongoCollection<IdentityRole> rolesCollection)
        {
            _rolesCollection = rolesCollection;
        }

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            _rolesCollection.InsertOne(role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            _rolesCollection.DeleteOne(r => r.Id == role.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_rolesCollection.Find(r => r.Id == roleId).FirstOrDefault());
        }

        public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_rolesCollection.Find(r => r.NormalizedName == normalizedRoleName).FirstOrDefault());
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            _rolesCollection.ReplaceOne(r => r.Id == role.Id, role);
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
            // No resource to dispose
        }
    }
}
