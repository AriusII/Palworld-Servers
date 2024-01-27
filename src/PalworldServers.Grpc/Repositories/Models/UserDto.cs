namespace PalworldServers.Grpc.Repositories.Models;

public sealed record UserDto(
    Guid Uuid,
    string Username,
    string Password,
    string Email,
    bool IsAdmin,
    bool IsBlocked,
    bool IsDeleted)
    : ISqlDataMapper<UserDto>
{
    public static UserDto MapFromReader(SqlDataReader reader)
    {
        return new UserDto(
            reader.GetGuid(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetBoolean(4),
            reader.GetBoolean(5),
            reader.GetBoolean(6)
        );
    }
}