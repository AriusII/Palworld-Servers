using GrpcAuthenticationRequest;
using GrpcAuthenticationResponse;
using Microsoft.AspNetCore.Authorization;
using PalworldServers.Grpc.Services.Interfaces;
using static GrpcAuthenticationService.AuthenticationService;

namespace PalworldServers.Grpc.Implementations.Authentications;

public sealed class AuthenticationImpl(IAuthService authenticationService) : AuthenticationServiceBase
{
    [AllowAnonymous]
    public override async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request,
        ServerCallContext context)
    {
        var userWithToken =
            await authenticationService.AuthenticateUser(request.Method.UsernameOrEmail, request.Password);

        return userWithToken;
    }
}