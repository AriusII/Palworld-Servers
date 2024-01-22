ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/PalworldServer.Grpc/PalworldServer.Grpc.csproj", "src/PalworldServer.Grpc/"]
RUN dotnet restore "src/PalworldServer.Grpc/PalworldServer.Grpc.csproj"
COPY . .
WORKDIR "/src/src/PalworldServer.Grpc"
RUN dotnet build "PalworldServer.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "PalworldServer.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PalworldServer.Grpc.dll"]
