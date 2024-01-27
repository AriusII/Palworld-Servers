using GrpcUserRequest;
using GrpcUserResponse;

namespace PalworldServers.Grpc.Services.Interfaces;

public interface IUsersService
{
    Task<GetUsersResponse> GetUsers(GetUsersRequest request);
    Task<GetUserResponse> GetUser(GetUserRequest request);
    Task<CreateUserResponse> CreateUser(CreateUserRequest request);
    Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request);
    Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request);
}