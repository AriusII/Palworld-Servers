namespace PalworldServers.Grpc.Repositories.Models;

public sealed record CheckUserDto(short Type) : ISqlDataMapper<CheckUserDto>
{
    public static CheckUserDto MapFromReader(SqlDataReader reader)
    {
        return new CheckUserDto(reader.GetInt16(0));
    }
}