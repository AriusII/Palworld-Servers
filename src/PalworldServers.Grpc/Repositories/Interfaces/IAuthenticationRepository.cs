using PalworldServers.Grpc.Repositories.Models.Enums;

namespace PalworldServers.Grpc.Repositories.Interfaces;

public interface IAuthenticationRepository
{
    Task<bool> CheckAccountEmailIsUniqueSql(string email);
    Task<bool> CheckAccountUsernameIsUniqueSql(string username);
    Task<bool> CreateUserSql(string email, string username, string password);
    Task<CheckAuthenticationUserErrors> CheckUserCredentialSql(string email, string password);
    Task<string> GetUserPasswordSql(string email);
}