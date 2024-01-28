using PalworldServers.Mail.Factory;
using PalworldServers.Mail.Models;
using PalworldServers.Mail.Repositories.Interfaces;
using PalworldServers.Mail.Services.Interfaces;

namespace PalworldServers.Mail.Services;

public sealed record EmailService(
    IEmailClientFactory EmailClientFactory,
    IEmailRepository EmailRepository)
    : IEmailService
{
    public Task<bool> SendEmailAsync(EmailPayloadDto emailPayloadDto)
    {
        var smtpClient = EmailClientFactory.CreateSmtpClient();
        var messageSender = EmailRepository.SendMailAsync(emailPayloadDto)
            .ContinueWith(
                task =>
                {
                    var message = task.Result;
                    smtpClient.Send(message);
                    return message;
                });

        return messageSender.ContinueWith(task => true);
    }
}