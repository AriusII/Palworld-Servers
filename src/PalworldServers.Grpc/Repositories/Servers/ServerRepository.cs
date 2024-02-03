using PalworldServers.Grpc.Commons.Models.Server;
using PalworldServers.Grpc.Repositories.Interfaces;
using PalworldServers.Grpc.Repositories.Models;
using ServerInformationsDto = PalworldServers.Grpc.Commons.Models.Server.ServerInformationsDto;

namespace PalworldServers.Grpc.Repositories.Servers;

public sealed record ServerRepository(ICaeriusDbConnectionFactory ConnectionFactory)
    : IServerRepository
{
    public async Task<ServerGuidDto> CreateServerSql(
        ServerInformationsDto serverInformationDto,
        ServerGameplayDto serverGameplayDto,
        ServerRateDto serverRateDto)
    {
        var spParameters = new StoredProcedureBuilder("Servers.sp_Create_New_Server")
            .AddParameter("@ServerName", serverInformationDto.ServerName, SqlDbType.NVarChar)
            .AddParameter("@ServerDescription", serverInformationDto.ServerDescription, SqlDbType.NVarChar)
            .AddParameter("@ServerIpAddress", serverInformationDto.ServerIpAddress, SqlDbType.NVarChar)
            .AddParameter("@WebsiteUrl", serverInformationDto.WebsiteUrl, SqlDbType.NVarChar)
            .AddParameter("@EmailOwner", serverInformationDto.EmailOwner, SqlDbType.NVarChar)
            .AddParameter("@ServerPlatform", serverGameplayDto.GamePlatform, SqlDbType.TinyInt)
            .AddParameter("@ServerType", serverGameplayDto.ServerType, SqlDbType.TinyInt)
            .AddParameter("@PlayersMax", serverGameplayDto.PlayersMax, SqlDbType.Int)
            .AddParameter("@ExperienceRate", serverRateDto.ExperienceRate, SqlDbType.Decimal)
            .AddParameter("@DropRate", serverRateDto.DropRate, SqlDbType.Decimal)
            .AddParameter("@TamingRate", serverRateDto.TamingRate, SqlDbType.Decimal)
            .AddParameter("@DaySpeed", serverRateDto.DaySpeed, SqlDbType.Decimal)
            .AddParameter("@NightSpeed", serverRateDto.NightSpeed, SqlDbType.Decimal)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<ServerGuidDto>(spParameters);
        return dbResult;
    }

    public async Task<ServerInformationsRepoDto> GetServerByUuidSql(Guid serverUuid)
    {
        var spParameters = new StoredProcedureBuilder("Servers.sp_Get_Server_By_Uuid")
            .AddParameter("@ServerUuid", serverUuid, SqlDbType.UniqueIdentifier)
            .Build();

        var dbResult = await ConnectionFactory.FirstOrDefaultAsync<ServerInformationsRepoDto>(spParameters);
        return dbResult;
    }

    public async Task<ImmutableArray<ServerInformationsRepoDto>> GetAListOfServersSql(int offset)
    {
        var spParameters = new StoredProcedureBuilder("Servers.sp_Get_A_List_Of_Servers")
            .AddParameter("@Offset", offset, SqlDbType.Int)
            .Build();

        var dbResult = await ConnectionFactory.QueryAsync<ServerInformationsRepoDto>(spParameters);
        return dbResult;
    }
}