using PalworldServers.Mail.Models;

namespace PalworldServers.Mail.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailPayloadDto emailPayloadDto);
}