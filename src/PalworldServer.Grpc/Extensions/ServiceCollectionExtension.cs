using PalworldServer.Grpc.Repositories.Interfaces;
using PalworldServer.Grpc.Repositories.Users;
using PalworldServer.Grpc.Services.Interfaces;
using PalworldServer.Grpc.Services.Users;

namespace PalworldServer.Grpc.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IUsersService, UsersService>();
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddScoped<IUsersRepository, UsersRepository>();
    }
}