using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Repositories.Models.Authentication.Register;
using PalworldServers.Grpc.Repositories.Models.Enums;

namespace PalworldServers.Grpc.Repositories.Authentication;

public sealed record AuthenticationRepository(ICaeriusDbConnectionFactory ConnectionFactory)
    : IAuthenticationRepository
{
    public async Task<bool> CheckAccountEmailIsUniqueSql(string email)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Check_If_Email_Is_Unique")
            .AddParameter("@Email", email, SqlDbType.NVarChar)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<UniqueEmailDto>(spParameters);
        return dbResult.IsUnique;
    }

    public async Task<bool> CheckAccountUsernameIsUniqueSql(string username)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Check_If_Username_Is_Unique")
            .AddParameter("@Username", username, SqlDbType.NVarChar)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<UniqueUsernameDto>(spParameters);
        return dbResult.IsUnique;
    }

    public async Task<bool> CreateUserSql(string email, string username, string password)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Create_New_Account")
            .AddParameter("@Email", email, SqlDbType.NVarChar)
            .AddParameter("@Username", username, SqlDbType.NVarChar)
            .AddParameter("@Password", password, SqlDbType.NVarChar)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<AccountCreated>(spParameters);
        return dbResult.IsCreated;
    }

    public async Task<CheckAuthenticationUserErrors> CheckUserCredentialSql(string email, string password)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Check_User_Credentials")
            .AddParameter("@Email", email, SqlDbType.NVarChar)
            .AddParameter("@Password", password, SqlDbType.NVarChar)
            .Build();

        var type = await ConnectionFactory.FirstOrDefaultAsync<CheckUserDto>(spParameters);
        var castType = (CheckAuthenticationUserErrors)type.Type;

        return castType;
    }

    public async Task<string> GetUserPasswordSql(string email)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Get_User_Password")
            .AddParameter("@Email", email, SqlDbType.NVarChar)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<UserAccountPasswordDto>(spParameters);
        return dbResult.Password;
    }
}

public sealed record UserAccountPasswordDto(string Password) : ISqlDataMapper<UserAccountPasswordDto>
{
    public static UserAccountPasswordDto MapFromReader(SqlDataReader reader)
    {
        var password = reader.GetString(0);
        return new UserAccountPasswordDto(password);
    }
}