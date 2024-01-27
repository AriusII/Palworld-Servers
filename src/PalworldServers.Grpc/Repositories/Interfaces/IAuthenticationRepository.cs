using PalworldServers.Grpc.Repositories.Models.Enums;

namespace PalworldServers.Grpc.Repositories.Interfaces;

public interface IAuthenticationRepository
{
    Task<CheckAuthenticationUserErrors> CheckUserCredentialSql(string username, string password);
}