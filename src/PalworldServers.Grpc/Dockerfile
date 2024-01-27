FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8100

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PalworldServers.Grpc/PalworldServers.Grpc.csproj", "src/PalworldServers.Grpc/"]
RUN dotnet restore "src/PalworldServers.Grpc/PalworldServers.Grpc.csproj"
COPY . .
WORKDIR "/src/PalworldServers.Grpc"
RUN dotnet build "PalworldServers.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PalworldServers.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PalworldServers.Grpc.dll"]
