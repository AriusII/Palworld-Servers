using GrpcUserModel;
using GrpcUserRequest;
using GrpcUserResponse;
using PalworldServer.Grpc.Repositories.Interfaces;

namespace PalworldServer.Grpc.Repositories.Users;

public sealed record UsersRepository(IConfiguration Configuration)
    : IUsersRepository
{
    private readonly string _connectionString = Configuration.GetConnectionString("PalworldSql")!;

    public async Task<GetUserResponse> GetUserSql(GetUserRequest request)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Guid", request.Uuid);

        var user = await connection.QuerySingleOrDefaultAsync("Authentication.sp_Get_User",
            parameters,
            commandType: CommandType.StoredProcedure);

        if (user == null)
            return new GetUserResponse
            {
                User = new User
                {
                    Uuid = Guid.Empty.ToString(),
                    Username = "",
                    Email = "",
                    Password = "",
                    IsBlocked = false,
                    IsAdmin = false
                }
            };

        var response = new GetUserResponse
        {
            User = new User
            {
                Uuid = request.Uuid,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                IsBlocked = user.IsBlocked,
                IsAdmin = user.IsAdmin
            }
        };

        return response;
    }
}