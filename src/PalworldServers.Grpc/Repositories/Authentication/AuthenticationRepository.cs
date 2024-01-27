using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Repositories.Models.Enums;

namespace PalworldServers.Grpc.Repositories.Authentication;

public sealed record AuthenticationRepository(ICaeriusDbConnectionFactory ConnectionFactory)
    : IAuthenticationRepository
{
    public Task<CheckAuthenticationUserErrors> CheckUserCredentialSql(string username, string password)
    {
        var spParameters = new StoredProcedureBuilder("Authentication.sp_Check_User_Credentials")
            .AddParameter("@Username", username, SqlDbType.NVarChar)
            .AddParameter("@Password", password, SqlDbType.NVarChar)
            .Build();

        var type = ConnectionFactory.FirstOrDefaultAsync<CheckUserDto>(spParameters)
            .ContinueWith(task =>
            {
                if (task.Result == null) return CheckAuthenticationUserErrors.UserNotFound;

                return task.Result.Type switch
                {
                    0 => CheckAuthenticationUserErrors.UserNotFound,
                    1 => CheckAuthenticationUserErrors.UserPasswordIncorrect,
                    2 => CheckAuthenticationUserErrors.UserCredentialsCorrect,
                    _ => CheckAuthenticationUserErrors.UserNotFound
                };
            });

        return type;
    }
}