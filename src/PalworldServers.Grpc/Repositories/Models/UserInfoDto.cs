namespace PalworldServers.Grpc.Repositories.Models;

public sealed record UserInfoDto(Guid Uuid, string Username, string Password, string Email);