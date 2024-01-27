namespace PalworldServers.Grpc.Repositories.Models;

public sealed record ServerDto(Guid NewServerUuid)
    : ISqlDataMapper<ServerDto>
{
    public static ServerDto MapFromReader(SqlDataReader reader)
    {
        var serverGuid = reader.GetGuid(0);

        return new ServerDto(serverGuid);
    }
}