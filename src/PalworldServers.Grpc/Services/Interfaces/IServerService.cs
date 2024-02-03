using GrpcServerRequest;
using GrpcServerResponse;

namespace PalworldServers.Grpc.Services.Interfaces;

public interface IServerService
{
    Task<CreateServerResponse> CreateServer(CreateServerRequest request);
    Task<GetServerListFromStreamResponse> GetServerListFromStream(GetServerListFromStreamRequest request);
}