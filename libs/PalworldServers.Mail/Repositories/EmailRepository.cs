using MimeKit;
using PalworldServers.Mail.Models;
using PalworldServers.Mail.Repositories.Interfaces;

namespace PalworldServers.Mail.Repositories;

public sealed record EmailRepository : IEmailRepository
{
    public async Task<MimeMessage> SendMailAsync(EmailPayloadDto emailPayloadDto)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sender", "hello@palworld-servers.com"));
        message.To.Add(new MailboxAddress("Receiver", emailPayloadDto.To));
        message.Subject = emailPayloadDto.Subject;
        message.Body = new TextPart("plain")
        {
            Text = emailPayloadDto.Body
        };

        return await Task.FromResult(message);
    }
}