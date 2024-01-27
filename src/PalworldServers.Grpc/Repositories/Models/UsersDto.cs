namespace PalworldServers.Grpc.Repositories.Models;

public sealed record UsersDto(ImmutableArray<UserDto> Users)
    : ISqlDataMapper<UsersDto>
{
    public static UsersDto MapFromReader(SqlDataReader reader)
    {
        var users = ImmutableArray.CreateBuilder<UserDto>();

        while (reader.Read()) users.Add(UserDto.MapFromReader(reader));

        return new UsersDto(users.ToImmutable());
    }
}