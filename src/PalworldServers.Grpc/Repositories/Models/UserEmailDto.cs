namespace PalworldServers.Grpc.Repositories.Models;

public sealed record UserEmailDto(Guid UserGuid)
    : ISqlDataMapper<UserEmailDto>
{
    public static UserEmailDto MapFromReader(SqlDataReader reader)
    {
        return new UserEmailDto(reader.GetGuid(0));
    }
}