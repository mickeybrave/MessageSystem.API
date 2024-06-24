using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MessageSystem.API.Security.Model;
using MessageSystem.API.Security.Settings;

namespace MessageSystem.API.Security
{
    public static class IdentityConfig
    {
        public static void AddIdentityConfig(this IServiceCollection services, IOptions<MongoDbConfigurationSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

            services.AddSingleton<IMongoCollection<IdentityUser>>(database.GetCollection<IdentityUser>("Users"));
            services.AddSingleton<IMongoCollection<IdentityRole>>(database.GetCollection<IdentityRole>("Roles"));

            services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
                .AddRoles<IdentityRole>()
                .AddUserStore<CustomUserStore>()
                .AddRoleStore<CustomRoleStore>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(1));
        }
    }
}
