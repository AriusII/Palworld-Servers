using GrpcServerRequest;
using GrpcServerResponse;
using PalworldServers.Grpc.Services.Interfaces;
using static GrpcServerService.ServerService;

namespace PalworldServers.Grpc.Implementations.Servers;

public sealed class ServerImpl(IServerService serverService) : ServerServiceBase
{
    public override async Task<CreateServerResponse> CreateServer(CreateServerRequest request,
        ServerCallContext context)
    {
        var result = await serverService.CreateServer(request);
        return result;
    }

    public override Task<GetServerInfoResponse> GetServerInfo(GetServerInfoRequest request, ServerCallContext context)
    {
        return base.GetServerInfo(request, context);
    }

    public override async Task GetServerListFromStream(
        GetServerListFromStreamRequest request,
        IServerStreamWriter<GetServerListFromStreamResponse> responseStream,
        ServerCallContext context)
    {
        var servers = await serverService.GetServerListFromStream(request);

        foreach (var server in servers.ServerInformationModel)
            await responseStream.WriteAsync(new GetServerListFromStreamResponse
            {
                ServerInformationModel = { server }
            });
    }
}