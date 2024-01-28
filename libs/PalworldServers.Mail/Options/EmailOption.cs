namespace PalworldServers.Mail.Options;

public sealed record EmailOption(string Host, int SmtpPort, int ImapPort, int PopPort, string Mail, string Password);