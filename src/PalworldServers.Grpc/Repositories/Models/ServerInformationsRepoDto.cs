using PalworldServers.Grpc.Repositories.Models.Enums;
using Guid = System.Guid;

namespace PalworldServers.Grpc.Repositories.Models;

public sealed record ServerInformationsRepoDto(
    Guid ServerUuid,
    string ServerName,
    string ServerDescription,
    string WebsiteUrl,
    string ServerIpAddress,
    ServerType ServerType,
    string ServerRate,
    int ServerViews,
    int ServerUpvotes,
    int ServerDownvotes,
    bool ServerIsVip,
    bool ServerIsDeleted)
    : ISqlDataMapper<ServerInformationsRepoDto>
{
    public static ServerInformationsRepoDto MapFromReader(SqlDataReader reader)
    {
        var serverUuid = reader.GetGuid(0);
        var serverName = reader.GetString(1);
        var serverDescription = reader.GetString(2);
        var websiteUrl = reader.GetString(3);
        var serverIpAddress = reader.GetString(4);
        var serverType = (ServerType)reader.GetInt32(5);
        var serverRate = reader.GetString(6);
        var serverViews = reader.GetInt32(7);
        var serverUpvotes = reader.GetInt32(8);
        var serverDownvotes = reader.GetInt32(9);
        var serverIsVip = reader.GetBoolean(10);
        var serverIsDeleted = reader.GetBoolean(11);

        return new ServerInformationsRepoDto(
            serverUuid,
            serverName,
            serverDescription,
            websiteUrl,
            serverIpAddress,
            serverType,
            serverRate,
            serverViews,
            serverUpvotes,
            serverDownvotes,
            serverIsVip,
            serverIsDeleted);
    }
}