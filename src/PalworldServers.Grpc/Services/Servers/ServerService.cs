using GrpcServerModel;
using GrpcServerRequest;
using GrpcServerResponse;
using PalworldServers.Grpc.Extensions;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Services.Interfaces;
using PalworldServers.Grpc.Services.Models;

namespace PalworldServers.Grpc.Services.Servers;

public sealed record ServerService(IServerRepository ServerRepository)
    : IServerService
{
    public async Task<CreateServerResponse> CreateServer(CreateServerRequest request)
    {
        var userGuid = GuidExtension.Convert(request.CreateServerModel.UserUuid);
        var serverDto = new CreateServerDto(userGuid);

        var serverInformationDto = new CreateServerInformationDto(
            request.CreateServerModel.ServerName,
            request.CreateServerModel.ServerDescription,
            request.CreateServerModel.ServerIpAddress);

        var server2 = await ServerRepository.CreateServerSql(serverDto, serverInformationDto);

        var server = await ServerRepository.GetServerByUuidSql(server2.NewServerUuid);

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
}