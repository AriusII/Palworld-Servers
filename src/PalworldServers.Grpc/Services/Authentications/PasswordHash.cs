using static BCrypt.Net.BCrypt;

namespace PalworldServers.Grpc.Services.Authentications;

public static class PasswordHasher
{
    private const int WorkFactor = 12;

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public static bool VerifyPassword(string password, string correctHash)
    {
        return Verify(password, correctHash);
    }
}