using GrpcUserRequest;
using GrpcUserResponse;
using PalworldServers.Grpc.Extensions;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Interfaces;
using User = GrpcUserModel.User;

namespace PalworldServers.Grpc.Services.Accounts;

public sealed record AccountService(IAccountRepository AccountRepository)
    : IUsersService
{
    public async Task<GetUsersResponse> GetUsers(GetUsersRequest request)
    {
        var dbResult = await AccountRepository.GetUsersSql(request.Page, request.Limit);

        var users = dbResult.Users.Select(user => new User
        {
            Guid = user.Uuid.ToString(),
            Username = user.Username,
            Email = user.Email,
            Password = user.Password,
            IsAdmin = user.IsAdmin,
            IsBlocked = user.IsBlocked,
            IsDeleted = user.IsDeleted
        }).ToList();

        return new GetUsersResponse
        {
            Users = { users },
            Total = users.Count
        };
    }

    public async Task<GetUserResponse> GetUser(GetUserRequest request)
    {
        var userGuid = GuidExtension.Convert(request.Guid);

        var dbResult = await AccountRepository.GetUserSql(userGuid);

        return new GetUserResponse
        {
            User = new User
            {
                Guid = dbResult.Uuid.ToString(),
                Username = dbResult.Username,
                Email = dbResult.Email,
                Password = dbResult.Password,
                IsAdmin = dbResult.IsAdmin,
                IsBlocked = dbResult.IsBlocked,
                IsDeleted = dbResult.IsDeleted
            }
        };
    }

    public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var userEmail = await CheckIfEmailIsAlreadyInUse(request.Email);
        if (userEmail.IsInUse) return new CreateUserResponse();

        var dbResult =
            await AccountRepository.CreateUserSql(new CreateUserDto(request.Username, request.Password, request.Email));

        return new CreateUserResponse
        {
            User = new User
            {
                Guid = dbResult.Uuid.ToString(),
                Username = dbResult.Username,
                Email = dbResult.Email,
                Password = dbResult.Password,
                IsAdmin = dbResult.IsAdmin,
                IsBlocked = dbResult.IsBlocked,
                IsDeleted = dbResult.IsDeleted
            }
        };
    }

    public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
    {
        var userGuid = GuidExtension.Convert(request.Guid);

        var dbResult = await AccountRepository.UpdateUserSql(
            new UserInfoDto(userGuid, request.Username, request.Password, request.Email));

        return new UpdateUserResponse
        {
            User = new User
            {
                Guid = dbResult.Uuid.ToString(),
                Username = dbResult.Username,
                Email = dbResult.Email,
                Password = dbResult.Password,
                IsAdmin = dbResult.IsAdmin,
                IsBlocked = dbResult.IsBlocked,
                IsDeleted = dbResult.IsDeleted
            }
        };
    }

    public async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request)
    {
        var userGuid = GuidExtension.Convert(request.Guid);

        var dbResult = await AccountRepository.DeleteUserSql(userGuid);

        return new DeleteUserResponse
        {
            User = new User
            {
                Guid = dbResult.Uuid.ToString(),
                Username = dbResult.Username,
                Email = dbResult.Email,
                Password = dbResult.Password,
                IsAdmin = dbResult.IsAdmin,
                IsBlocked = dbResult.IsBlocked,
                IsDeleted = dbResult.IsDeleted
            }
        };
    }

    private async Task<UserEmailDto> CheckIfEmailIsAlreadyInUse(string email)
    {
        var userEmailIsInUse = await AccountRepository.CheckIfEmailIsAlreadyInUseSql(email);

        return userEmailIsInUse.IsInUse
            ? new UserEmailDto(true)
            : new UserEmailDto(false);
    }
}