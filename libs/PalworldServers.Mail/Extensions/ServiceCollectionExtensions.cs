using Microsoft.Extensions.DependencyInjection;
using PalworldServers.Mail.Factory;
using PalworldServers.Mail.Options;
using PalworldServers.Mail.Repositories;
using PalworldServers.Mail.Repositories.Interfaces;
using PalworldServers.Mail.Services;
using PalworldServers.Mail.Services.Interfaces;

namespace PalworldServers.Mail.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMailKit(this IServiceCollection services, EmailOption configuration)
    {
        return services
            .AddScoped<IEmailClientFactory, EmailClientFactory>(x => new EmailClientFactory(configuration))
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IEmailRepository, EmailRepository>();
    }
}