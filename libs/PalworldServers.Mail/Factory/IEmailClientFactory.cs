using MailKit.Net.Imap;
using MailKit.Net.Smtp;

namespace PalworldServers.Mail.Factory;

public interface IEmailClientFactory
{
    SmtpClient CreateSmtpClient();
    ImapClient CreateImapClient();
}