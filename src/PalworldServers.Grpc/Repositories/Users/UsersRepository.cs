using Caerius.Orm.Commands.Writes;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;

namespace PalworldServers.Grpc.Repositories.Users;

public sealed record UsersRepository(ICaeriusDbConnectionFactory ConnectionFactory)
    : IUsersRepository
{
    public Task<UsersDto> GetUsersSql(int page, int limit)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Get_Users")
            .AddParameter("@Page", page, SqlDbType.Int)
            .AddParameter("@Limit", limit, SqlDbType.Int)
            .Build();

        return ConnectionFactory.FirstOrDefaultAsync<UsersDto>(spParameters);
    }

    public async Task<UserDto> GetUserSql(Guid userGuid)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Get_User")
            .AddParameter("@Guid", userGuid, SqlDbType.UniqueIdentifier)
            .Build();

        return await ConnectionFactory.FirstOrDefaultAsync<UserDto>(spParameters);
    }

    public async Task<UserDto> CreateUserSql(CreateUserDto createUserDto)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Create_User")
            .AddParameter("@Username", createUserDto.Username, SqlDbType.NVarChar)
            .AddParameter("@Password", createUserDto.Password, SqlDbType.NVarChar)
            .AddParameter("@Email", createUserDto.Email, SqlDbType.NVarChar)
            .Build();

        return await ConnectionFactory.FirstOrDefaultAsync<UserDto>(spParameters);
    }

    public async Task<UserEmailDto> CheckIfEmailIsAlreadyInUseSql(string email)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Check_If_Email_Exits")
            .AddParameter("@Email", email, SqlDbType.NVarChar)
            .Build();

        return await ConnectionFactory.FirstOrDefaultAsync<UserEmailDto>(spParameters);
    }

    public async Task<UserDto> UpdateUserSql(UserInfoDto userInfoDto)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Update_User")
            .AddParameter("@Guid", userInfoDto.Uuid, SqlDbType.UniqueIdentifier)
            .AddParameter("@Username", userInfoDto.Username, SqlDbType.NVarChar)
            .AddParameter("@Password", userInfoDto.Password, SqlDbType.NVarChar)
            .AddParameter("@Email", userInfoDto.Email, SqlDbType.NVarChar)
            .Build();

        await ConnectionFactory.ExecuteAsync(spParameters);

        return await GetUserSql(userInfoDto.Uuid);
    }

    public async Task<UserDto> DeleteUserSql(Guid userGuid)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Delete_User")
            .AddParameter("@Guid", userGuid, SqlDbType.UniqueIdentifier)
            .Build();

        await ConnectionFactory.ExecuteAsync(spParameters);

        return await GetUserSql(userGuid);
    }
}