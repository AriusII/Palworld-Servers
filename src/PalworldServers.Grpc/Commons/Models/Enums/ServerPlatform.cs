namespace PalworldServers.Grpc.Commons.Models.Enums;

[Flags]
public enum GamePlatform
{
    Undefined = 0,
    Pc = 2,
    Xbox = 4,
    GamePass = 8,
    Ps4 = 16,
    Ps5 = 32,
    Switch = 64
}