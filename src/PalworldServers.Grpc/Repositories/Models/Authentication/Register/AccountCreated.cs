namespace PalworldServers.Grpc.Repositories.Models.Authentication.Register;

public sealed record AccountCreated(bool IsCreated) : ISqlDataMapper<AccountCreated>
{
    public static AccountCreated MapFromReader(SqlDataReader reader)
    {
        return new AccountCreated(reader.GetBoolean(0));
    }
}