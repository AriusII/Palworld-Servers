using GrpcAuthenticationRequest;
using GrpcAuthenticationResponse;

namespace PalworldServers.Grpc.Services.Interfaces;

public interface IAuthService
{
    Task<AuthenticationResponse> AuthenticateUser(string email, string password);
    Task RegisterAccount(RegisterRequest request);
}