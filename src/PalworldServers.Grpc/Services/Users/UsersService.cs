using GrpcUserRequest;
using GrpcUserResponse;
using PalworldServers.Grpc.Extensions;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Interfaces;
using User = GrpcUserModel.User;

namespace PalworldServers.Grpc.Services.Users;

public sealed record UsersService(IUsersRepository UsersRepository)
    : IUsersService
{
    public async Task<GetUsersResponse> GetUsers(GetUsersRequest request)
    {
        var dbResult = await UsersRepository.GetUsersSql(request.Page, request.Limit);

        var users = dbResult.Users.Select(user => new User
        {
            Uuid = user.Uuid.ToString(),
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
        var userGuid = GuidExtension.Convert(request.Uuid);

        var dbResult = await UsersRepository.GetUserSql(userGuid);

        if (dbResult is null)
            return new GetUserResponse
            {
                User = new User
                {
                    Uuid = Guid.Empty.ToString(),
                    Username = string.Empty,
                    Password = string.Empty,
                    Email = string.Empty,
                    IsAdmin = false,
                    IsBlocked = false,
                    IsDeleted = false
                }
            };

        return new GetUserResponse
        {
            User = new User
            {
                Uuid = dbResult.Uuid.ToString(),
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
            await UsersRepository.CreateUserSql(new CreateUserDto(request.Username, request.Password, request.Email));

        return new CreateUserResponse
        {
            User = new User
            {
                Uuid = dbResult.Uuid.ToString(),
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
        var userGuid = GuidExtension.Convert(request.Uuid);

        var dbResult = await UsersRepository.UpdateUserSql(
            new UserInfoDto(userGuid, request.Username, request.Password, request.Email));

        return new UpdateUserResponse
        {
            User = new User
            {
                Uuid = dbResult.Uuid.ToString(),
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
        var userGuid = GuidExtension.Convert(request.Uuid);

        var dbResult = await UsersRepository.DeleteUserSql(userGuid);

        return new DeleteUserResponse
        {
            User = new User
            {
                Uuid = dbResult.Uuid.ToString(),
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
        var userEmailIsInUse = await UsersRepository.CheckIfEmailIsAlreadyInUseSql(email);

        return userEmailIsInUse.IsInUse
            ? new UserEmailDto(true)
            : new UserEmailDto(false);
    }
}