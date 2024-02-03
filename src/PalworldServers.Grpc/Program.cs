using System.Security.Authentication;
using System.Text;
using Caerius.Orm.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PalworldServers.Grpc.Extensions;
using PalworldServers.Grpc.Implementations.Accounts;
using PalworldServers.Grpc.Implementations.Authentications;
using PalworldServers.Grpc.Implementations.Sandbox;
using PalworldServers.Grpc.Implementations.Servers;
using PalworldServers.Mail.Extensions;
using PalworldServers.Mail.Options;

var builder = WebApplication.CreateBuilder(args);

var sysKey = builder.Configuration.GetSection("Token").GetSection("Key").Value!;
var emailSettings = builder.Configuration.GetSection("Email").Get<EmailOption>()!;

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ConfigureHttpsDefaults(httpsOptions => { httpsOptions.SslProtocols = SslProtocols.Tls13; });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://id.palworld-servers.com",
            ValidAudience = "https://grpc.palworld-servers.com"
        };
        options.Configuration = new OpenIdConnectConfiguration
        {
            SigningKeys = { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sysKey)) }
        };
    });


builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
    .AddPolicy("User", policy => policy.RequireRole("User"));

// Add services to the container.
builder.Services
    .AddLogging()
    .RegisterServices(builder.Configuration)
    .RegisterRepositories()
    .AddCaerius("Prod")
    .RegisterMailKit(emailSettings)
    .AddGrpc();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AuthenticationImpl>();
app.MapGrpcService<UsersImpl>();
app.MapGrpcService<ServerImpl>();
app.MapGrpcService<SandboxImpl>();

// Configure the HTTP request pipeline.
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();