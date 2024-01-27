using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using PalworldServers.Grpc.Services.Models;

namespace PalworldServers.Grpc.Repositories.Servers;

public sealed record ServerRepository(ICaeriusDbConnectionFactory ConnectionFactory)
    : IServerRepository
{
    public async Task<ServerDto> CreateServerSql(
        CreateServerDto serverDto,
        CreateServerInformationDto serverInformationDto)
    {
        var spParameters = new StoredProcedureBuilder("Servers.sp_Create_New_Server")
            .AddParameter("@UserUuid", serverDto.UserGuid, SqlDbType.UniqueIdentifier)
            .AddParameter("@ServerName", serverInformationDto.ServerName, SqlDbType.NVarChar)
            .AddParameter("@ServerDescription", serverInformationDto.ServerDescription, SqlDbType.NVarChar)
            .AddParameter("@ServerIpAddress", serverInformationDto.ServerAddressIp, SqlDbType.NVarChar)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<ServerDto>(spParameters);
        return dbResult;
    }

    public async Task<ServerInformationsDto> GetServerByUuidSql(Guid serverUuid)
    {
        var spParameters = new StoredProcedureBuilder("Servers.sp_Get_Server_By_Uuid")
            .AddParameter("@ServerUuid", serverUuid, SqlDbType.UniqueIdentifier)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<ServerInformationsDto>(spParameters);
        return dbResult;
    }

    public async Task<ImmutableArray<ServerInformationsDto>> GetAListOfServersSql(int offset)
    {
        var spParameters = new StoredProcedureBuilder("Servers.sp_Get_A_List_Of_Servers")
            .AddParameter("@Offset", offset, SqlDbType.Int)
            .Build();

        var dbResult = await ConnectionFactory.QueryAsync<ServerInformationsDto>(spParameters);
        return dbResult;
    }
}