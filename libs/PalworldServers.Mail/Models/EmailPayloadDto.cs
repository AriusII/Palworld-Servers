namespace PalworldServers.Mail.Models;

public sealed record EmailPayloadDto(string To, string Subject, string Body);