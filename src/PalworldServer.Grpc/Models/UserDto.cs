namespace PalworldServer.Grpc.Models;

public sealed record UserDto(
    Guid Uuid,
    string Username,
    string Password,
    string Email,
    bool IsAdmin,
    bool IsBlocked);