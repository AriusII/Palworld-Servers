using GrpcAuthenticationRequest;
using GrpcAuthenticationResponse;
using PalworldServers.Grpc.Services.Interfaces;
using static GrpcAuthenticationService.AuthenticationService;

namespace PalworldServers.Grpc.Implementations.Authentications;

public sealed class AuthenticationImpl(
    IAuthService authenticationService,
    ILogger<AuthenticationImpl> logger)
    : AuthenticationServiceBase
{
    [AllowAnonymous]
    public override async Task<AuthenticationResponse> Login(AuthenticationRequest request,
        ServerCallContext context)
    {
        logger.LogInformation("Login called with {Credentials}", request.Credentials.Email);
        var userWithToken =
            await authenticationService.AuthenticateUser(request.Credentials.Email, request.Credentials.Password);

        return userWithToken;
    }

    [AllowAnonymous]
    public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        logger.LogInformation("Register called with : {RegisterForm}", request.Register);
        await authenticationService.RegisterAccount(request);

        return new RegisterResponse();
    }
}