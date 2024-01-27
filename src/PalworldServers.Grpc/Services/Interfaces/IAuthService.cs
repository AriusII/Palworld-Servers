using GrpcAuthenticationResponse;

namespace PalworldServers.Grpc.Services.Interfaces;

public interface IAuthService
{
    Task<AuthenticationResponse> AuthenticateUser(string username, string password);
}