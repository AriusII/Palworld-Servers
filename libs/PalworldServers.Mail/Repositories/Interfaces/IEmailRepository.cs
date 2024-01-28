using MimeKit;
using PalworldServers.Mail.Models;

namespace PalworldServers.Mail.Repositories.Interfaces;

public interface IEmailRepository
{
    Task<MimeMessage> SendMailAsync(EmailPayloadDto emailPayloadDto);
}