using GrpcServerRequest;
using GrpcServerResponse;

namespace PalworldServers.Grpc.Services.Interfaces;

public interface IServerService
{
    Task<CreateServerResponse> CreateServerAndGetBackit(CreateServerRequest request);
    Task<GetServerListFromStreamResponse> GetServerListFromStream(GetServerListFromStreamRequest request);
}