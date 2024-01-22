using GrpcUserRequest;
using GrpcUserResponse;
using PalworldServer.Grpc.Repositories.Interfaces;
using PalworldServer.Grpc.Services.Interfaces;

namespace PalworldServer.Grpc.Services.Users;

public sealed record UsersService(IUsersRepository UsersRepository)
    : IUsersService
{
    public async Task<GetUserResponse> GetUser(GetUserRequest request)
    {
        return await UsersRepository.GetUserSql(request);
    }
}