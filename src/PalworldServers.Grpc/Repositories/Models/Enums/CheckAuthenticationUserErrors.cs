namespace PalworldServers.Grpc.Repositories.Models.Enums;

public enum CheckAuthenticationUserErrors
{
    UserNotFound = 0,
    UserPasswordIncorrect = 1,
    UserCredentialsCorrect = 2
}