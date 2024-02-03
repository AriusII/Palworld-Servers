using PalworldServers.Grpc.Repositories.Models;

namespace PalworldServers.Grpc.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<UserGuidDto> CreateNewAccountWithEmail(string email);
    Task<UsersDto> GetUsersSql(int page, int limit);
    Task<UserDto> GetUserSql(Guid userGuid);
    Task<UserDto> CreateUserSql(CreateUserDto userInfoDto);
    Task<UserEmailDto> CheckIfEmailIsAlreadyInUseSql(string email);
    Task<UserDto> UpdateUserSql(UserInfoDto userInfoDto);
    Task<UserDto> DeleteUserSql(Guid userGuid);
}