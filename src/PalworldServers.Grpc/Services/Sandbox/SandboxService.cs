using GrpcSandboxService;
using PalworldServers.Mail.Models;
using PalworldServers.Mail.Services.Interfaces;

namespace PalworldServers.Grpc.Services.Sandbox;

public sealed record SandboxService(IEmailService EmailService) : ISandboxService
{
    public async Task<TestSendEmailResponse> TestSendEmail(TestSendEmailRequest request)
    {
        var emailRequest = new EmailPayloadDto(request.UserEmail, request.UserEmailSubject, request.UserEmailBody);

        var response = await EmailService.SendEmailAsync(emailRequest);

        if (response)
            return new TestSendEmailResponse
            {
                IsSended = true
            };

        return new TestSendEmailResponse
        {
            IsSended = false
        };
    }
}