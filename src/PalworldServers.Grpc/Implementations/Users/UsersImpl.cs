using GrpcUserRequest;
using GrpcUserResponse;
using Microsoft.AspNetCore.Authorization;
using PalworldServers.Grpc.Services.Interfaces;
using static GrpcUserService.UserService;

namespace PalworldServers.Grpc.Implementations.Users;

public sealed class UsersImpl(IUsersService usersService, ILogger<UsersImpl> logger)
    : UserServiceBase
{
    [AllowAnonymous]
    public override async Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
    {
        logger.LogInformation("GetUsers called from Page {Page} and Offset {Offset}", request.Page, request.Limit);
        return await usersService.GetUsers(request);
    }


    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        logger.LogInformation("GetUser called with {Uuid}", request.Uuid);
        return await usersService.GetUser(request);
    }

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        logger.LogInformation("CreateUser called with {Email}", request.Email);
        return await usersService.CreateUser(request);
    }

    public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        logger.LogInformation("UpdateUser called with {Username}", request.Username);
        return await usersService.UpdateUser(request);
    }

    public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        logger.LogInformation("DeleteUser called with {Uuid}", request.Uuid);
        return await usersService.DeleteUser(request);
    }
}