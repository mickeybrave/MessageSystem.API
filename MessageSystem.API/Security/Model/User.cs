using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace MessageSystem.API.Security.Model
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<Guid>
    {
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        //public IList<string> Roles { get; set; } = new List<string>(); // Roles can be "User" or "Admin"
    }


    //[CollectionName("Users")]
    //public class ApplicationUser : MongoIdentityUser<Guid>
    //{
    //    public ApplicationUser() : base() { }
    //}
}
