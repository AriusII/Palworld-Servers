namespace PalworldServers.Grpc.Services.Models;

public sealed record CreateServerInformationDto(string ServerName, string ServerDescription, string ServerAddressIp);