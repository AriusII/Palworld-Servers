using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Models;

namespace PalworldServers.Grpc.Repositories.Interfaces;

public interface IServerRepository
{
    Task<ServerDto> CreateServerSql(CreateServerDto serverDto,
        CreateServerInformationDto serverInformationDto);

    Task<ServerInformationsDto> GetServerByUuidSql(Guid serverUuid);
    Task<ImmutableArray<ServerInformationsDto>> GetAListOfServersSql(int offset);
}