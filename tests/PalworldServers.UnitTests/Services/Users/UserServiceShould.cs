using System.Collections.Immutable;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Accounts;
using User = GrpcUserModel.User;

namespace PalworldServers.UnitTests.Services.Users;

public sealed class UserServiceShould
{
    private static readonly AccountService AccountService = UserServiceBuilder.Build();
    private static readonly Guid GoodUserGuid = Guid.NewGuid();
    private static readonly Guid BadUserGuid = Guid.NewGuid();

    [Fact]
    public async void Return_A_List_Of_Users_From_A_Page_And_Limit()
    {
        // Arrange
        const int page = 1;
        const int limit = 10;

        var fakerDbUser = new Faker<UserDto>()
            .CustomInstantiator(f => new UserDto(
                f.Random.Guid(),
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Random.Bool()));

        var listOfUsers = fakerDbUser.Generate(limit);

        UsersDto users = new(listOfUsers.ToImmutableArray());


        // Act
        UserServiceBuilder.SetupGetUsersSql(page, limit, users);

        // Assert
        var myTest = await AccountService.GetUsers(new GetUsersRequest { Page = 1, Limit = 10 });

        myTest
            .Should()
            .BeOfType<GetUsersResponse>().Subject
            .Should()
            .BeEquivalentTo(new GetUsersResponse
            {
                Users =
                {
                    listOfUsers.Select(user => new User
                    {
                        Uuid = user.Uuid.ToString(),
                        Username = user.Username,
                        Email = user.Email,
                        Password = user.Password,
                        IsAdmin = user.IsAdmin,
                        IsBlocked = user.IsBlocked,
                        IsDeleted = user.IsDeleted
                    }).ToList()
                },
                Total = listOfUsers.Count
            });
    }

    [Fact]
    public async void Return_A_Good_User()
    {
        // Arrange
        var fakerDbUser = new Faker<UserDto>()
            .CustomInstantiator(f => new UserDto(
                f.Random.Guid(),
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Random.Bool()));

        var dbUser = fakerDbUser.Generate();

        var goodUser = new User
        {
            Uuid = dbUser.Uuid.ToString(),
            Username = dbUser.Username,
            Email = dbUser.Email,
            Password = dbUser.Password,
            IsAdmin = dbUser.IsAdmin,
            IsBlocked = dbUser.IsBlocked,
            IsDeleted = dbUser.IsDeleted
        };

        // Act
        UserServiceBuilder.SetupGetUserSql(GoodUserGuid, dbUser);

        // Assert
        var myTest = await AccountService.GetUser(new GetUserRequest { Uuid = GoodUserGuid.ToString() });

        myTest
            .Should()
            .BeOfType<GetUserResponse>().Subject
            .Should()
            .BeEquivalentTo(new GetUserResponse
            {
                User = goodUser
            });
    }

    [Fact]
    public async void Return_A_Not_Existent_User()
    {
        // Arrange
        var noDbUser = new UserDto(
            Guid.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            false,
            false,
            false);

        var badUser = new User
        {
            Uuid = noDbUser.Uuid.ToString(),
            Username = noDbUser.Username,
            Email = noDbUser.Email,
            Password = noDbUser.Password,
            IsAdmin = noDbUser.IsAdmin,
            IsBlocked = noDbUser.IsBlocked,
            IsDeleted = noDbUser.IsDeleted
        };

        // Act
        UserServiceBuilder.SetupGetUserSql(BadUserGuid, noDbUser);

        // Assert
        var myTest = await AccountService.GetUser(new GetUserRequest { Uuid = BadUserGuid.ToString() });

        myTest
            .Should()
            .BeOfType<GetUserResponse>().Subject
            .Should()
            .BeEquivalentTo(new GetUserResponse
            {
                User = badUser
            });
    }

    [Fact]
    public async void Return_A_Created_User()
    {
        // Arrange
        var fakerNewUser = new Faker<CreateUserDto>()
            .CustomInstantiator(f => new CreateUserDto(
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email));
        var newUser = fakerNewUser.Generate();


        var fakerDbUser = new Faker<UserDto>()
            .CustomInstantiator(f => new UserDto(
                f.Random.Guid(),
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Random.Bool()));
        var dbUser = fakerDbUser.Generate();

        var goodUser = new User
        {
            Uuid = dbUser.Uuid.ToString(),
            Username = dbUser.Username,
            Email = dbUser.Email,
            Password = dbUser.Password,
            IsAdmin = dbUser.IsAdmin,
            IsBlocked = dbUser.IsBlocked,
            IsDeleted = dbUser.IsDeleted
        };

        // Act
        UserServiceBuilder.SetupCreateUserSql(newUser, dbUser);

        // Assert
        var myTest = await AccountService.CreateUser(new CreateUserRequest
        {
            Username = newUser.Username,
            Email = newUser.Email,
            Password = newUser.Password
        });

        myTest
            .Should()
            .BeOfType<CreateUserResponse>().Subject
            .Should()
            .BeEquivalentTo(new CreateUserResponse
            {
                User = goodUser
            });
    }

    [Fact]
    public async void Return_An_User_Email_Already_Registered()
    {
        // Arrange
        var fakerNewUser = new Faker<CreateUserDto>()
            .CustomInstantiator(f => new CreateUserDto(
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email));
        var newUser = fakerNewUser.Generate();

        var userEmailIsInUse = new UserEmailDto(true);


        // Act
        UserServiceBuilder.SetupCreateUserSql_ReturnEmailAlreadyRegistered(newUser.Email, userEmailIsInUse);

        // Assert
        var myTest = await AccountService.CreateUser(new CreateUserRequest
        {
            Username = newUser.Username,
            Email = newUser.Email,
            Password = newUser.Password
        });

        myTest
            .Should()
            .BeOfType<CreateUserResponse>().Subject
            .Should()
            .BeEquivalentTo(new CreateUserResponse());
    }

    [Fact]
    public async void Return_An_Updated_User()
    {
        // Arrange
        var fakerToUpdateUser = new Faker<UserInfoDto>()
            .CustomInstantiator(f => new UserInfoDto(
                f.Random.Guid(),
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email));
        var userToUpdate = fakerToUpdateUser.Generate();


        var fakerDbUser = new Faker<UserDto>()
            .CustomInstantiator(f => new UserDto(
                f.Random.Guid(),
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Random.Bool()));
        var dbUser = fakerDbUser.Generate();

        var goodUser = new User
        {
            Uuid = dbUser.Uuid.ToString(),
            Username = dbUser.Username,
            Email = dbUser.Email,
            Password = dbUser.Password,
            IsAdmin = dbUser.IsAdmin,
            IsBlocked = dbUser.IsBlocked,
            IsDeleted = dbUser.IsDeleted
        };

        // Act
        UserServiceBuilder.SetupUpdateUserSql(userToUpdate, dbUser);

        // Assert
        var myTest = await AccountService.UpdateUser(new UpdateUserRequest
        {
            Uuid = userToUpdate.Uuid.ToString(),
            Username = userToUpdate.Username,
            Email = userToUpdate.Email,
            Password = userToUpdate.Password
        });

        myTest
            .Should()
            .BeOfType<UpdateUserResponse>().Subject
            .Should()
            .BeEquivalentTo(new UpdateUserResponse
            {
                User = goodUser
            });
    }

    [Fact]
    public async void Return_A_Deleted_User()
    {
        // Arrange
        var fakerUserToDelete = new Faker<UserDto>()
            .CustomInstantiator(f => new UserDto(
                f.Random.Guid(),
                f.Person.UserName,
                f.Random.String2(20),
                f.Person.Email,
                f.Random.Bool(),
                f.Random.Bool(),
                true));
        var dbUser = fakerUserToDelete.Generate();

        var deletedUser = new User
        {
            Uuid = dbUser.Uuid.ToString(),
            Username = dbUser.Username,
            Email = dbUser.Email,
            Password = dbUser.Password,
            IsAdmin = dbUser.IsAdmin,
            IsBlocked = dbUser.IsBlocked,
            IsDeleted = true
        };

        // Act
        UserServiceBuilder.SetupDeleteUserSql(GoodUserGuid, dbUser);

        // Assert
        var myTest = await AccountService.DeleteUser(new DeleteUserRequest { Uuid = GoodUserGuid.ToString() });

        myTest
            .Should()
            .BeOfType<DeleteUserResponse>().Subject
            .Should()
            .BeEquivalentTo(new DeleteUserResponse
            {
                User = deletedUser
            });
    }
}