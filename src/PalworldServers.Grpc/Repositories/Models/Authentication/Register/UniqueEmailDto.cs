namespace PalworldServers.Grpc.Repositories.Models.Authentication.Register;

public sealed record UniqueEmailDto(bool IsUnique)
    : ISqlDataMapper<UniqueEmailDto>
{
    public static UniqueEmailDto MapFromReader(SqlDataReader reader)
    {
        return new UniqueEmailDto(reader.GetBoolean(0));
    }
}