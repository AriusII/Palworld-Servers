namespace PalworldServers.Grpc.Repositories.Models;

public sealed record ServerGuidDto(Guid NewServerUuid)
    : ISqlDataMapper<ServerGuidDto>
{
    public static ServerGuidDto MapFromReader(SqlDataReader reader)
    {
        var serverGuid = reader.GetGuid(0);

        return new ServerGuidDto(serverGuid);
    }
}