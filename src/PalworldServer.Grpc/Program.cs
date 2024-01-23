using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PalworldServer.Grpc.Extensions;
using PalworldServer.Grpc.Implementations.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
    .AddEnvironmentVariables();

builder.WebHost
    .UseKestrel()
    .ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 12123, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });

// Add services to the container.
builder.Services
    .AddLogging()
    .RegisterServices()
    .RegisterRepositories(builder.Configuration)
    .AddGrpc();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapGrpcService<UsersImpl>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();