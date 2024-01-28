using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using PalworldServers.Mail.Options;

namespace PalworldServers.Mail.Factory;

public sealed record EmailClientFactory(EmailOption EmailOption)
    : IEmailClientFactory
{
    public SmtpClient CreateSmtpClient()
    {
        var smtpClient = new SmtpClient();
        smtpClient.Connect(EmailOption.Host, EmailOption.SmtpPort, true);
        smtpClient.Authenticate(EmailOption.Mail, EmailOption.Password);
        return smtpClient;
    }

    public ImapClient CreateImapClient()
    {
        var imapClient = new ImapClient();
        imapClient.Connect(EmailOption.Host, EmailOption.ImapPort, true);
        imapClient.Authenticate(EmailOption.Mail, EmailOption.Password);
        return imapClient;
    }
}