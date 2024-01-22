using GrpcUserRequest;
using GrpcUserResponse;

namespace PalworldServer.Grpc.Repositories.Interfaces;

public interface IUsersRepository
{
    Task<GetUserResponse> GetUserSql(GetUserRequest request);
}