using PalworldServers.Grpc.Repositories.Authentication;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Servers;
using PalworldServers.Grpc.Repositories.Users;
using PalworldServers.Grpc.Services.Authentications;
using PalworldServers.Grpc.Services.Interfaces;
using PalworldServers.Grpc.Services.Sandbox;
using PalworldServers.Grpc.Services.Servers;
using PalworldServers.Grpc.Services.Users;

namespace PalworldServers.Grpc.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthService, AuthenticationService>()
            .AddScoped<IUsersService, UsersService>()
            .AddScoped<IServerService, ServerService>()
            .AddScoped<ISandboxService, SandboxService>();
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthenticationRepository, AuthenticationRepository>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IServerRepository, ServerRepository>();
    }
}