namespace PalworldServers.Grpc.Repositories.Models;

public sealed record UserGuidDto(
    Guid UserGuid)
    : ISqlDataMapper<UserGuidDto>
{
    public static UserGuidDto MapFromReader(SqlDataReader reader)
    {
        return new UserGuidDto(
            reader.GetGuid(0)
        );
    }
}