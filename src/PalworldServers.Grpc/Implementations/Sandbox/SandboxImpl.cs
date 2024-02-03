using GrpcSandboxService;
using PalworldServers.Grpc.Services.Sandbox;

namespace PalworldServers.Grpc.Implementations.Sandbox;

public class SandboxImpl(ISandboxService sandboxService) : GrpcSandboxService.GrpcSandboxService.GrpcSandboxServiceBase
{
    public override async Task<TestSendEmailResponse> TestSendEmail(TestSendEmailRequest request,
        ServerCallContext context)
    {
        var response = sandboxService.TestSendEmail(request);
        return await response;
    }
}