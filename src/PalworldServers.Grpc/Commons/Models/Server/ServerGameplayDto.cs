using PalworldServers.Grpc.Commons.Models.Enums;

namespace PalworldServers.Grpc.Commons.Models.Server;

public sealed record ServerGameplayDto(GamePlatform GamePlatform, ServerType ServerType, int PlayersMax);