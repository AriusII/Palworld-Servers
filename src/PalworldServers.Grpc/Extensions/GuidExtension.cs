namespace PalworldServers.Grpc.Extensions;

public static class GuidExtension
{
    public static Guid Convert(string uuid)
    {
        var convertStringToGuid = Guid.TryParse(uuid, out var guid);
        return convertStringToGuid
            ? guid
            : Guid.Empty;
    }
}