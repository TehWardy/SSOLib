using System;
using Security.Objects.Entities;
using Moq;
using Xunit;

namespace Security.Services.Tests.Processing
{
	public partial class SSOUserProcessingServiceTests
	{
		[Fact]
		public void MeShouldWorkAsExpected()
		{
			//given
			SSOUser expectedSSOUser = RandomSSOUser();

			identityBrokerMock.Setup(identityBrokerMock =>
				identityBrokerMock.Me())
				.Returns(expectedSSOUser);

			//when
			SSOUser actualSSOUser = ssoUserProcessingService.Me();

			//then
			identityBrokerMock.Verify(identityBrokerMock =>
				identityBrokerMock.Me(),
				Times.Once());

			ssoUserServiceMock.VerifyNoOtherCalls();
			passwordEncryptionBrokerMock.VerifyNoOtherCalls();
		}
	}
}

