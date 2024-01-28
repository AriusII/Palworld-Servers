using GrpcServerModel;
using GrpcServerRequest;
using GrpcServerResponse;
using PalworldServers.Grpc.Extensions;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Interfaces;
using PalworldServers.Grpc.Services.Models;

namespace PalworldServers.Grpc.Services.Servers;

public sealed record ServerService(IServerRepository ServerRepository)
    : IServerService
{
    public async Task<CreateServerResponse> CreateServerAndGetBackit(CreateServerRequest request)
    {
        var createdServerWithGuid = await CreateServer(request);

        var createdServerWithInformation = await GetCreatedServer(createdServerWithGuid);

        return createdServerWithInformation;
    }

    public async Task<GetServerListFromStreamResponse> GetServerListFromStream(GetServerListFromStreamRequest request)
    {
        var servers = await ServerRepository.GetAListOfServersSql(request.Offset);

        var serversMapping = new List<ServerInformationModel>();

        foreach (var server in servers)
        {
            var serverInformation = new ServerInformationModel
            {
                ServerUuid = server.ServerUuid.ToString(),
                ServerName = server.ServerName,
                ServerDescription = server.ServerDescription,
                WebsiteUrl = server.WebsiteUrl,
                ServerIpAddress = server.ServerIpAddress,
                ServerType = (int)server.ServerType,
                ServerRate = server.ServerRate,
                ServerViews = server.ServerViews,
                ServerUpvotes = server.ServerUpvotes,
                ServerDownvotes = server.ServerDownvotes,
                ServerIsVip = server.ServerIsVip,
                ServerIsDeleted = server.ServerIsDeleted
            };

            serversMapping.Add(serverInformation);
        }

        var grpcResponse = new GetServerListFromStreamResponse
        {
            ServerInformationModel = { serversMapping }
        };

        return grpcResponse;
    }

    private async Task<ServerDto> CreateServer(CreateServerRequest request)
    {
        var userGuid = GuidExtension.Convert(request.CreateServerModel.UserUuid);
        var serverDto = new CreateServerDto(userGuid);

        var serverInformationDto = new CreateServerInformationDto(
            request.CreateServerModel.ServerName,
            request.CreateServerModel.ServerDescription,
            request.CreateServerModel.ServerIpAddress);

        var createdServerWithGuid = await ServerRepository.CreateServerSql(serverDto, serverInformationDto);
        return createdServerWithGuid;
    }

    private async Task<CreateServerResponse> GetCreatedServer(ServerDto serverGuid)
    {
        var server = await ServerRepository.GetServerByUuidSql(serverGuid.NewServerUuid);

        var serverInformation = new ServerInformationModel
        {
            ServerUuid = server.ServerUuid.ToString(),
            ServerName = server.ServerName,
            ServerDescription = server.ServerDescription,
            WebsiteUrl = server.WebsiteUrl,
            ServerIpAddress = server.ServerIpAddress,
            ServerType = (int)server.ServerType,
            ServerRate = server.ServerRate,
            ServerViews = server.ServerViews,
            ServerUpvotes = server.ServerUpvotes,
            ServerDownvotes = server.ServerDownvotes,
            ServerIsVip = server.ServerIsVip,
            ServerIsDeleted = server.ServerIsDeleted
        };

        var grpcResponse = new CreateServerResponse
        {
            ServerInformationModel = serverInformation
        };

        return grpcResponse;
    }
}