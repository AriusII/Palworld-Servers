using PalworldServers.Grpc.Commons.Models.Server;
using PalworldServers.Grpc.Repositories.Models;
using ServerInformationsDto = PalworldServers.Grpc.Commons.Models.Server.ServerInformationsDto;

namespace PalworldServers.Grpc.Repositories.Interfaces;

public interface IServerRepository
{
    Task<ServerGuidDto> CreateServerSql(
        ServerInformationsDto serverInformationsDto,
        ServerGameplayDto serverGameplayDto,
        ServerRateDto serverRateDto);

    Task<ServerInformationsRepoDto> GetServerByUuidSql(Guid serverUuid);
    Task<ImmutableArray<ServerInformationsRepoDto>> GetAListOfServersSql(int offset);
}