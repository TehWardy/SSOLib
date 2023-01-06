using FizzWare.NBuilder;
using System;
using Moq;
using Security.Data.Brokers.Authentication;
using Security.Data.Brokers.Encryption;
using Security.Objects.Entities;
using Security.Services.Processing;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using Tynamix.ObjectFiller;
using System.Linq;

namespace Security.Services.Tests.Processing
{
	public partial class SSOUserProcessingServiceTests
	{
		private readonly Mock<IIdentityBroker> identityBrokerMock;
		private readonly Mock<IPasswordEncryptionBroker> passwordEncryptionBrokerMock;
		private readonly Mock<ISSOUserService> ssoUserServiceMock;
		private readonly ISSOUserProcessingService ssoUserProcessingService;

		public SSOUserProcessingServiceTests()
		{
			identityBrokerMock = new Mock<IIdentityBroker>();
			passwordEncryptionBrokerMock = new Mock<IPasswordEncryptionBroker>();
			ssoUserServiceMock = new Mock<ISSOUserService>();
			ssoUserProcessingService = new SSOUserProcessingService(ssoUserServiceMock.Object,
				passwordEncryptionBrokerMock.Object,
				identityBrokerMock.Object);
		}

		static string RandomString()
			=> new MnemonicString().GetValue();

		static SSOUser[] RandomSSOUsers()
			=> Enumerable.Range(1, new Random().Next(10, 20))
				.Select(_ => RandomSSOUser())
				.ToArray();

		static SSOUser RandomSSOUser()
			=> GetSSOUserFiller().Create();

		static Filler<SSOUser> GetSSOUserFiller()
		{
			var filler = new Filler<SSOUser>();
			filler.Setup()
				.OnProperty(p => p.LockoutEnabled).Use(false)
				.OnProperty(p => p.Roles).IgnoreIt()
				.OnProperty(p => p.Tokens).IgnoreIt()
				.OnProperty(p => p.UserEvents).IgnoreIt();

			return filler;
		}
	}
}

