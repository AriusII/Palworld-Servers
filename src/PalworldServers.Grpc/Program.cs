using System.Security.Authentication;
using System.Text;
using Caerius.Orm.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PalworldServers.Grpc.Extensions;
using PalworldServers.Grpc.Implementations.Authentications;
using PalworldServers.Grpc.Implementations.Sandbox;
using PalworldServers.Grpc.Implementations.Servers;
using PalworldServers.Grpc.Implementations.Users;
using PalworldServers.Grpc.Services.Interfaces;
using PalworldServers.Grpc.Services.Tokens;
using PalworldServers.Mail.Extensions;
using PalworldServers.Mail.Options;

var builder = WebApplication.CreateBuilder(args);

var sysKey = builder.Configuration.GetSection("Token")["Key"]!;
var emailSettings = builder.Configuration.GetSection("Email").Get<EmailOption>()!;

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ConfigureHttpsDefaults(httpsOptions => { httpsOptions.SslProtocols = SslProtocols.Tls13; });
});

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sysKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
    .AddPolicy("User", policy => policy.RequireRole("User"));

// Add services to the container.
builder.Services
    .AddLogging()
    .AddSingleton<ITokenService>(new TokenService(sysKey))
    .RegisterServices()
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