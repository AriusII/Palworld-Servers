using GrpcSandboxService;
using PalworldServers.Grpc.Services.Sandbox;
using static GrpcSandboxService.GrpcSandboxService;

namespace PalworldServers.Grpc.Implementations.Sandbox;

public class SandboxImpl(ISandboxService sandboxService) : GrpcSandboxServiceBase
{
    public override async Task<TestSendEmailResponse> TestSendEmail(TestSendEmailRequest request,
        ServerCallContext context)
    {
        var response = sandboxService.TestSendEmail(request);
        return await response;
    }
}