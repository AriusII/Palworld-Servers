FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 12123

COPY ./publish/ .

ENTRYPOINT ["dotnet", "PalworldServer.Grpc.dll"]