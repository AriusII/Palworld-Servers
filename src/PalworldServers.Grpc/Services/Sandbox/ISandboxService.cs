using GrpcSandboxService;

namespace PalworldServers.Grpc.Services.Sandbox;

public interface ISandboxService
{
    Task<TestSendEmailResponse> TestSendEmail(TestSendEmailRequest request);
}