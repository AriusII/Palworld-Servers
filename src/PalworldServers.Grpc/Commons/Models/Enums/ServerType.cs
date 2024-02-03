namespace PalworldServers.Grpc.Commons.Models.Enums;

[Flags]
public enum ServerType
{
    Undentified = 0,
    PvE = 2,
    PvP = 4,
    Hardcord = 8,
    Roleplay = 16,
    Creative = 32,
    Vanilla = 64,
    Modded = 128
}