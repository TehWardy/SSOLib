using FluentAssertions;
using Moq;
using Security.Objects.Entities;
using System.Linq;
using System.Security;
using Xunit;

namespace Security.Services.Tests.Processing
{
    public partial class SSOUserProcessingServiceTests
	{
		[Fact]
		public void FindByUserAndPasswordWorksAsExpected()
		{
			//given
			string inputPassword = RandomString();

			IQueryable<SSOUser> ssoUsersInService = RandomSSOUsers()
				.AsQueryable();

			ssoUserServiceMock.Setup(ssoUserServiceMock =>
				ssoUserServiceMock.GetAllSSOUsers(true))
				.Returns(ssoUsersInService);

            SSOUser expectedSSOUser = ssoUsersInService.First();

            passwordEncryptionBrokerMock.Setup(passwordEncryptionBrokerMock =>
				passwordEncryptionBrokerMock.EncryptedAndPlainTextAreEqual(expectedSSOUser.PasswordHash, inputPassword))
				.Returns(true);

			//when
			SSOUser actualSSOUser = ssoUserProcessingService.FindByUserAndPassword(expectedSSOUser.Id, inputPassword);

			//then
			actualSSOUser.Should().BeEquivalentTo(expectedSSOUser);

			ssoUserServiceMock.Verify(ssoUserServiceMock =>
				ssoUserServiceMock.GetAllSSOUsers(true),
				Times.Once);

			passwordEncryptionBrokerMock.Verify(passwordEncryptionBrokerMock =>
				passwordEncryptionBrokerMock.EncryptedAndPlainTextAreEqual(expectedSSOUser.PasswordHash, inputPassword),
				Times.Once);
		}

        [Fact]
        public void FindByUserAndPasswordNotWorksForLockoutAsExpected()
        {
            //given
            string inputPassword = RandomString();

            IQueryable<SSOUser> ssoUsersInService = RandomSSOUsers()
                .AsQueryable();

            ssoUserServiceMock.Setup(ssoUserServiceMock =>
                ssoUserServiceMock.GetAllSSOUsers(true))
                .Returns(ssoUsersInService);

            SSOUser expectedSSOUser = ssoUsersInService.First();

            expectedSSOUser.LockoutEnabled = true;

            passwordEncryptionBrokerMock.Setup(passwordEncryptionBrokerMock =>
                passwordEncryptionBrokerMock.EncryptedAndPlainTextAreEqual(expectedSSOUser.PasswordHash, inputPassword))
                .Returns(true);

            //when & then
            Assert.Throws<SecurityException>(() => ssoUserProcessingService.FindByUserAndPassword(expectedSSOUser.Id, inputPassword));
        }
    }
}

