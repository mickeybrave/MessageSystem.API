namespace MessageSystem.API.Security.Settings
{
    public class MongoDbConfigurationSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; } = "Users";

    }
}
