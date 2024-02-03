namespace PalworldServers.Grpc.Repositories.Models.Authentication.Register;

public sealed record UniqueUsernameDto(bool IsUnique) : ISqlDataMapper<UniqueUsernameDto>
{
    public static UniqueUsernameDto MapFromReader(SqlDataReader reader)
    {
        return new UniqueUsernameDto(reader.GetBoolean(0));
    }
}