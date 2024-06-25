using AspNetCore.Identity.MongoDbCore.Infrastructure;
using MessageSystem.API.DAL;
using MessageSystem.API.Security.Model;
using MessageSystem.API.Security.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MessageSystem.API.Security
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }
        public IMongoCollection<IdentityRole> Roles => _database.GetCollection<IdentityRole>("Roles");

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<IdentityUser> IdentityUsers => _database.GetCollection<IdentityUser>("IdentityUsers");


        //public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");

    }

}
