using Microsoft.AspNetCore.Identity;

namespace PalworldServers.Grpc.Services.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(IdentityUser user);
}