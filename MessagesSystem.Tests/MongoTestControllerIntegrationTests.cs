using MessageSystem.API.Security.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MessagesSystem.Tests
{
    public class MongoTestControllerIntegrationTests2 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public MongoTestControllerIntegrationTests2(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                });

                // Retrieve connection string from environment variable
                var mongoConnectionString = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.User);

                builder.ConfigureTestServices(services =>
                {
                    services.Configure<MongoDbConfigurationSettings>(options =>
                    {
                        options.ConnectionString = mongoConnectionString;
                        options.DatabaseName = "Users";
                    });

                    services.AddSingleton<IMongoClient, MongoClient>(sp =>
                    {
                        var settings = sp.GetRequiredService<IOptions<MongoDbConfigurationSettings>>().Value;
                        return new MongoClient(settings.ConnectionString);
                    });

                    services.AddSingleton(sp =>
                    {
                        var settings = sp.GetRequiredService<IOptions<MongoDbConfigurationSettings>>().Value;
                        var client = sp.GetRequiredService<IMongoClient>();
                        return client.GetDatabase(settings.DatabaseName);
                    });

                    services.AddSingleton(sp =>
                    {
                        var database = sp.GetRequiredService<IMongoDatabase>();
                        return database.GetCollection<IdentityUser>("Users");
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Test_Get_All_Messages()
        {
            // Act - Send HTTP GET request to controller endpoint
            var response = await _client.GetAsync("/api/mongotest");

            // Assert - Check if the response is successful and contains expected data
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("count", responseString);
        }
    }
}
