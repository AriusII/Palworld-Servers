namespace PalworldServers.Grpc.Repositories.Models;

public sealed record CheckUserDto(int Type) : ISqlDataMapper<CheckUserDto>
{
    public static CheckUserDto MapFromReader(SqlDataReader reader)
    {
        return new CheckUserDto(reader.GetInt32(0));
    }
}