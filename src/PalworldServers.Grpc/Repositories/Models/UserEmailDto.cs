namespace PalworldServers.Grpc.Repositories.Models;

public sealed record UserEmailDto(bool IsInUse)
    : ISqlDataMapper<UserEmailDto>
{
    public static UserEmailDto MapFromReader(SqlDataReader reader)
    {
        return new UserEmailDto(reader.GetBoolean(0));
    }
}