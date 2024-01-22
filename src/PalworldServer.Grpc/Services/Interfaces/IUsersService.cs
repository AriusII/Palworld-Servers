using GrpcUserRequest;
using GrpcUserResponse;

namespace PalworldServer.Grpc.Services.Interfaces;

public interface IUsersService
{
    Task<GetUserResponse> GetUser(GetUserRequest request);
}