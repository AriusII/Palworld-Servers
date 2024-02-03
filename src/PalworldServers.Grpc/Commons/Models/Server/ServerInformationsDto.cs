namespace PalworldServers.Grpc.Commons.Models.Server;

public sealed record ServerInformationsDto(
    string ServerName,
    string ServerDescription,
    string ServerIpAddress,
    string WebsiteUrl,
    string EmailOwner);