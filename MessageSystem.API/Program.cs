using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MessageSystem.API.Security.Settings;
using MessageSystem.API.Security.Service;
using MessageSystem.API.Security;
using Microsoft.Extensions.Options;
using MessageSystem.API.Security.Model;
using MessageSystem.API.BL;
using MessageSystem.API.DAL;
using MessageSystem.API.BL.Auth;
using MessageSystem.API.DAL.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configure MongoDB settings

// Retrieve connection string from environment variable
var mongoConnectionString = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.User);

// Check if the connection string is null or empty
if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new InvalidOperationException("MongoDB connection string is not set in environment variables.");
}
// Add the connection string to the configuration
builder.Configuration["MongoDbConfigurationSettings:ConnectionString"] = mongoConnectionString;

// Register MongoDbContext
builder.Services.AddSingleton<MongoDbContext>();

// Register IMongoCollection<IdentityUser>
builder.Services.AddSingleton(sp =>
{
    var context = sp.GetRequiredService<MongoDbContext>();
    return context.Users;
});

// Configure Identity
builder.Services.AddIdentity<User, Role>()
    .AddMongoDbStores<User, Role, Guid>(
        mongoConnectionString,
        builder.Configuration["MongoDbConfigurationSettings:DatabaseName"]
    )
    .AddDefaultTokenProviders();


// Configure MongoDB settings
builder.Services.Configure<MongoDbConfigurationSettings>(builder.Configuration.GetSection("MongoDbConfigurationSettings"));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbConfigurationSettings"));

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbConfigurationSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Configure Identity
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<Role>()
    .AddMongoDbStores<User, Role, Guid>(
        mongoConnectionString,
        builder.Configuration["MongoDbConfigurationSettings:DatabaseName"]
    )
    .AddSignInManager()
    .AddDefaultTokenProviders();



// Configure JWT authentication
builder.Services.AddJwtConfig(builder.Configuration);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();
// Register AuthenticationService
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


//// Register IMongoCollection<IdentityUser>
builder.Services.AddSingleton(sp =>
{
    var context = sp.GetRequiredService<MongoDbContext>();
    return context.Users;
});

var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseRouting();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
