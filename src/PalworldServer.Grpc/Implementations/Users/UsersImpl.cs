using GrpcUserRequest;
using GrpcUserResponse;
using PalworldServer.Grpc.Services.Interfaces;
using static GrpcUserService.UserService;

namespace PalworldServer.Grpc.Implementations.Users;

public sealed class UsersImpl(IUsersService usersService, ILogger<UsersImpl> logger)
    : UserServiceBase
{
    public override Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
    {
        return base.GetUsers(request, context);
    }

    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        logger.LogInformation("GetUser called with Uuid: {Uuid}", request.Uuid);
        return await usersService.GetUser(request);
    }

    public override Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        return base.CreateUser(request, context);
    }

    public override Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return base.UpdateUser(request, context);
    }

    public override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return base.DeleteUser(request, context);
    }
}