using GrpcServerModel;
using GrpcServerRequest;
using GrpcServerResponse;
using PalworldServers.Grpc.Commons.Models.Enums;
using PalworldServers.Grpc.Commons.Models.Server;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Interfaces;
using PalworldServers.Mail.Factory;
using PalworldServers.Mail.Models;
using PalworldServers.Mail.Services.Interfaces;
using ServerInformationsDto = PalworldServers.Grpc.Commons.Models.Server.ServerInformationsDto;

namespace PalworldServers.Grpc.Services.Servers;

public sealed record ServerService(IServerRepository ServerRepository, IAccountRepository AccountRepository, IEmailService EmailService)
    : IServerService
{
    /*
    public async Task<CreateServerResponse> CreateServerAndGetBackit(CreateServerRequest request)
    {
        
        var createdServerWithGuid = await CreateServer(request);

        var createdServerWithInformation = await GetCreatedServer(createdServerWithGuid);

        return createdServerWithInformation;
        
    }

    public async Task<GetServerListFromStreamResponse> GetServerListFromStream(GetServerListFromStreamRequest request)
    {
        var servers = await ServerRepository.GetAListOfServersSql(request.Offset);

        var serverInformations = new List<>();

        var serverGameplay = new List<>();
        
        var serverRate = new List<>();
        
        var serversMapping = new List<ServerInformationModel>();

        foreach (var server in servers)
        {
            var serverInformation = new ServerInformationModel
            {
                ServerUuid = server.ServerUuid.ToString(),
                ServerName = server.ServerName,
                ServerDescription = server.ServerDescription,
                WebsiteUrl = server.WebsiteUrl,
                ServerIpAddress = server.ServerIpAddress,
                ServerType = (int)server.ServerType,
                ServerRate = server.ServerRate,
                ServerViews = server.ServerViews,
                ServerUpvotes = server.ServerUpvotes,
                ServerDownvotes = server.ServerDownvotes,
                ServerIsVip = server.ServerIsVip,
                ServerIsDeleted = server.ServerIsDeleted
            };

            serversMapping.Add(serverInformation);
        }

        var grpcResponse = new GetServerListFromStreamResponse
        {
            ServerInformationModel = { serversMapping }
        };

        return grpcResponse;
    }

    private async Task<ServerGuidDto> CreateServer(CreateServerRequest request)
    {
        var serverInfos = new ServerInformationsDto(
            request.CreateServerModel.ServerInformation.ServerName,
            request.CreateServerModel.ServerInformation.ServerDescription,
            request.CreateServerModel.ServerInformation.ServerIpAddress,
            request.CreateServerModel.ServerInformation.WebsiteUrl,
            request.CreateServerModel.ServerInformation.EmailOwner);

        var serverGameplay = new ServerGameplayDto(
            (GamePlatform)request.CreateServerModel.ServerGameplay.GamePlatform,
            (ServerType)request.CreateServerModel.ServerGameplay.ServerType,
            request.CreateServerModel.ServerGameplay.PlayersMax);

        var serverRate = new ServerRateDto(
            request.CreateServerModel.ServerRate.ExperienceRate,
            request.CreateServerModel.ServerRate.DropRate,
            request.CreateServerModel.ServerRate.TamingRate,
            request.CreateServerModel.ServerRate.DaySpeed,
            request.CreateServerModel.ServerRate.NightSpeed);


        var createdServerWithGuid = await ServerRepository.CreateServerSql(serverInfos, serverGameplay, serverRate);
        return createdServerWithGuid;
    }

    private async Task<CreateServerResponse> GetCreatedServer(ServerGuidDto serverGuidGuid)
    {
        var server = await ServerRepository.GetServerByUuidSql(serverGuidGuid.NewServerUuid);

        var serverInformation = new ServerInformationModel
        {
            ServerUuid = server.ServerUuid.ToString(),
            ServerName = server.ServerName,
            ServerDescription = server.ServerDescription,
            WebsiteUrl = server.WebsiteUrl,
            ServerIpAddress = server.ServerIpAddress,
            ServerType = (int)server.ServerType,
            ServerRate = server.ServerRate,
            ServerViews = server.ServerViews,
            ServerUpvotes = server.ServerUpvotes,
            ServerDownvotes = server.ServerDownvotes,
            ServerIsVip = server.ServerIsVip,
            ServerIsDeleted = server.ServerIsDeleted
        };

        var grpcResponse = new CreateServerResponse
        {
            ServerInformationModel = serverInformation
        };

        return grpcResponse;
    }
    */
    public async Task<CreateServerResponse> CreateServer(CreateServerRequest request)
    {
        var email = request.CreateServer.ServerInformations.EmailOwner;
        // check email deja present en db 1. si oui on l'attache au compte existant 2. si non on le crée
        var accountAlreadyExist = await AccountRepository.CheckIfEmailIsAlreadyInUseSql(email);
        if (accountAlreadyExist.UserGuid)
        {
            // envoie de confirmation si on reutilise son compte
        }
        else
        {
            // creation du compte
            var createdAccount = await AccountRepository.CreateNewAccountWithEmail(email);

            // envoie de confirmation de la création du server
            const string emailBody = "Votre compte a été créé avec succès, vous pouvez maintenant créer votre serveur";
            var emailRequest = new EmailPayloadDto(email, "Confirmation d'enregistrement de serveur", emailBody);

            await EmailService.SendEmailAsync(emailRequest);
        }
            
        
        
        // envoie de mail
        
        // creation du serveur, mais sur un etat de isDeleted
        
        return new CreateServerResponse();
    }

    public Task<GetServerListFromStreamResponse> GetServerListFromStream(GetServerListFromStreamRequest request)
    {
        throw new NotImplementedException();
    }
}