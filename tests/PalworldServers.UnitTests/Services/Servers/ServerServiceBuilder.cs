using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Models;
using PalworldServers.Grpc.Services.Servers;

namespace PalworldServers.UnitTests.Services.Servers;

public static class ServerServiceBuilder
{
    private static readonly Mock<IServerRepository> ServerRepositoryMock = new();

    public static ServerService Build()
    {
        return new ServerService(ServerRepositoryMock.Object);
    }

    public static void SetupCreateServerSql(
        CreateServerDto serverFromRequest,
        CreateServerInformationDto serverInformationFromRequest,
        ServerDto serverFromDatabase)
    {
        ServerRepositoryMock.Setup(r => r.CreateServerSql(serverFromRequest, serverInformationFromRequest))
            .ReturnsAsync(serverFromDatabase);
    }
}