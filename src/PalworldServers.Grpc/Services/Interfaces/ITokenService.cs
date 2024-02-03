namespace PalworldServers.Grpc.Services.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(string email);
}