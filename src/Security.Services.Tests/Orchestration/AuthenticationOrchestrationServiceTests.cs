using Moq;
using Security.Services.Orchestration;
using Security.Services.Orchestration.Interfaces;
using Security.Services.Processing.Interfaces;
using Tynamix.ObjectFiller;

namespace Security.Services.Tests.Orchestration
{
    public partial class AuthenticationOrchestrationServiceTests
    {
        private readonly Mock<ISessionProcessingService> sessionProcessingServiceMock;
        private readonly Mock<ITokenProcessingService> tokenProcessingServiceMock;
        private readonly Mock<ISSOUserProcessingService> ssoUserProcessingServiceMock;
        private readonly IAuthenticationOrchestrationService authenticationOrchestrationService;

        public AuthenticationOrchestrationServiceTests()
        {
            sessionProcessingServiceMock = new Mock<ISessionProcessingService>();
            tokenProcessingServiceMock = new Mock<ITokenProcessingService>();
            ssoUserProcessingServiceMock = new Mock<ISSOUserProcessingService>();
            authenticationOrchestrationService = new AuthenticationOrchestrationService(
                ssoUserProcessingServiceMock.Object,
                tokenProcessingServiceMock.Object,
                sessionProcessingServiceMock.Object);
        }

        static string RandomString()
            => new MnemonicString().GetValue();
    }
}
