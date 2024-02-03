using GrpcAuthenticationModel;
using GrpcAuthenticationRequest;
using GrpcAuthenticationResponse;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models.Enums;
using PalworldServers.Grpc.Services.Interfaces;

namespace PalworldServers.Grpc.Services.Authentications;

public sealed record AuthenticationService(
    IAuthenticationRepository AuthenticationRepository,
    ITokenService TokenService)
    : IAuthService
{
    public async Task RegisterAccount(RegisterRequest request)
    {
        await Task.WhenAll(
            CheckAccountEmailIsUnique(request.Register.Email),
            CheckAccountUsernameIsUnique(request.Register.Username)
        );

        var hashedPassword = PasswordHasher.HashPassword(request.Register.Password);
        var createdUser =
            await AuthenticationRepository.CreateUserSql(request.Register.Email, request.Register.Username,
                hashedPassword);
        if (!createdUser)
            throw new RpcException(new Status(StatusCode.Internal, "Something went wrong"));
    }

    public async Task<AuthenticationResponse> AuthenticateUser(string email, string password)
    {
        var checkedUser = await CheckAccountCredentials(email, password);
        switch (checkedUser)
        {
            case CheckAuthenticationUserErrors.UserNotFound:
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User not found"));

            case CheckAuthenticationUserErrors.UserPasswordIncorrect:
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User password incorrect"));

            case CheckAuthenticationUserErrors.UserCredentialsCorrect:

                var generatedToken = await GenerateJwtTokenForValidUser(email);

                return generatedToken;
            default:
                throw new RpcException(new Status(StatusCode.Internal, "Something went wrong"));
        }
    }

    private async Task CheckAccountEmailIsUnique(string email)
    {
        var emailIsUnique = await AuthenticationRepository.CheckAccountEmailIsUniqueSql(email);
        if (!emailIsUnique)
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Email already exists"));
    }

    private async Task CheckAccountUsernameIsUnique(string username)
    {
        var usernameIsUnique = await AuthenticationRepository.CheckAccountUsernameIsUniqueSql(username);
        if (!usernameIsUnique)
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Username already exists"));
    }

    private async Task<CheckAuthenticationUserErrors> CheckAccountCredentials(string email, string password)
    {
        var dbAccountPassword = await AuthenticationRepository.GetUserPasswordSql(email);
        var verifiedPassword = PasswordHasher.VerifyPassword(password, dbAccountPassword);

        if (!verifiedPassword)
            return CheckAuthenticationUserErrors.UserPasswordIncorrect;

        return await AuthenticationRepository.CheckUserCredentialSql(email, dbAccountPassword);
    }

    private Task<AuthenticationResponse> GenerateJwtTokenForValidUser(string email)
    {
        var token = TokenService.GenerateJwtToken(email);

        var userToken = new JwtToken
        {
            Token = token
        };

        var response = new AuthenticationResponse { Bearer = userToken };

        return Task.FromResult(response);
    }
}