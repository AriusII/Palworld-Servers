using GrpcAuthenticationModel;
using GrpcAuthenticationResponse;
using Microsoft.AspNetCore.Identity;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models.Enums;
using PalworldServers.Grpc.Services.Interfaces;

namespace PalworldServers.Grpc.Services.Authentications;

public sealed record AuthenticationService(
    IAuthenticationRepository AuthenticationRepository,
    ITokenService TokenService)
    : IAuthService
{
    public async Task<AuthenticationResponse> AuthenticateUser(string username, string password)
    {
        var checkedUser = await CheckUser(username, password);
        switch (checkedUser)
        {
            case CheckAuthenticationUserErrors.UserNotFound:
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User not found"));

            case CheckAuthenticationUserErrors.UserPasswordIncorrect:
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User password incorrect"));

            case CheckAuthenticationUserErrors.UserCredentialsCorrect:
                var identityUser = new IdentityUser(username);

                var token = TokenService.GenerateJwtToken(identityUser);

                var userToken = new JwtToken
                {
                    Token = token
                };

                var response = new AuthenticationResponse { Token = userToken };

                return response;
            default:
                throw new RpcException(new Status(StatusCode.Internal, "Something went wrong"));
        }
    }

    private async Task<CheckAuthenticationUserErrors> CheckUser(string username, string password)
    {
        return await AuthenticationRepository.CheckUserCredentialSql(username, password);
    }
}