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

        //public MongoDbContext(IOptions<MongoDbConfigurationSettings> settings)
        //{
        //    var client = new MongoClient(settings.Value.ConnectionString);
        //    _database = client.GetDatabase(settings.Value.DatabaseName);
        //}

        public MongoDbContext(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }
        public IMongoCollection<IdentityUser> Users => _database.GetCollection<IdentityUser>("Users");
        public IMongoCollection<IdentityRole> Roles => _database.GetCollection<IdentityRole>("Roles");
        //public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");

    }

}
