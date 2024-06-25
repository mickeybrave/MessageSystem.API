using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace MessageSystem.API.Security.Model
{
    [CollectionName("Roles")]
    public class Role : MongoIdentityRole<Guid>
    {
        [BsonElement("name")]
        public string Name { get; set; }
    }

    //[CollectionName("Roles")]
    //public class ApplicationRole : MongoIdentityRole<Guid>
    //{
    //    public ApplicationRole() : base() { }
    //    public ApplicationRole(string roleName) : base(roleName) { }
    //}


   
}
