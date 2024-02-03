using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Accounts;

namespace PalworldServers.UnitTests.Services.Users;

public static class UserServiceBuilder
{
    private static readonly Mock<IAccountRepository> UserRepositoryMock = new();

    public static AccountService Build()
    {
        return new AccountService(UserRepositoryMock.Object);
    }

    public static void SetupGetUsersSql(int page, int limit, UsersDto users)
    {
        UserRepositoryMock.Setup(r => r.GetUsersSql(page, limit))
            .ReturnsAsync(users);
    }

    public static void SetupGetUserSql(Guid userGuid, UserDto user)
    {
        UserRepositoryMock.Setup(r => r.GetUserSql(userGuid))
            .ReturnsAsync(user);
    }

    public static void SetupCreateUserSql(CreateUserDto userFromRequest, UserDto userFromDatabase)
    {
        UserRepositoryMock.Setup(r => r.CreateUserSql(userFromRequest))
            .ReturnsAsync(userFromDatabase);
    }

    public static void SetupCreateUserSql_ReturnEmailAlreadyRegistered(string userEmail, UserEmailDto userFromDatabase)
    {
        UserRepositoryMock.Setup(r => r.CheckIfEmailIsAlreadyInUseSql(userEmail))
            .ReturnsAsync(userFromDatabase);
    }

    public static void SetupUpdateUserSql(UserInfoDto userFromRequest, UserDto userFromDatabase)
    {
        UserRepositoryMock.Setup(r => r.UpdateUserSql(userFromRequest))
            .ReturnsAsync(userFromDatabase);
    }

    public static void SetupDeleteUserSql(Guid userGuid, UserDto userFromDatabase)
    {
        UserRepositoryMock.Setup(r => r.DeleteUserSql(userGuid))
            .ReturnsAsync(userFromDatabase);
    }
}