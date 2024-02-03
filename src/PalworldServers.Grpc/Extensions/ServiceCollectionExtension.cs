using PalworldServers.Grpc.Repositories.Accounts;
using PalworldServers.Grpc.Repositories.Authentication;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Servers;
using PalworldServers.Grpc.Services.Accounts;
using PalworldServers.Grpc.Services.Authentications;
using PalworldServers.Grpc.Services.Interfaces;
using PalworldServers.Grpc.Services.Sandbox;
using PalworldServers.Grpc.Services.Servers;
using PalworldServers.Grpc.Services.Tokens;

namespace PalworldServers.Grpc.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var sysKey = configuration.GetSection("Token").GetSection("Key").Value!;

        return services
            .AddSingleton<ITokenService, TokenService>(provider => new TokenService(sysKey))
            .AddScoped<IAuthService, AuthenticationService>()
            .AddScoped<IUsersService, AccountService>()
            .AddScoped<IServerService, ServerService>()
            .AddScoped<ISandboxService, SandboxService>();
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthenticationRepository, AuthenticationRepository>()
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IServerRepository, ServerRepository>();
    }
}