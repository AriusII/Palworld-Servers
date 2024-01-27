namespace PalworldServers.Grpc.Repositories.Models;

public sealed record CreateUserDto(string Username, string Password, string Email);