using FluentAssertions;
using Security.Objects.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
	public partial class SSOUserApiTests
	{
		[Fact]
		public async void ShouldGetAllSSOUsersAsync()
		{
			//given
			List<SSOUser> expectedSSOUsers = new List<SSOUser>();

			foreach (var registerUser in RandomRegisterUsers())
                expectedSSOUsers.Add(await registerApiClient.RegisterAsync(registerUser));

			//when
			IEnumerable<SSOUser> actualSSOUsers = await ssoUserApiClient.GetAllSSOUsersAsync();

			//then
			foreach (var expectedUser in expectedSSOUsers)
			{
				SSOUser actualUser = actualSSOUsers.FirstOrDefault(u => u.Id == expectedUser.Id);
				actualUser.Should().BeEquivalentTo(expectedUser);      
			}

			foreach (var ssoUser in expectedSSOUsers)
				await registerApiClient.TearDown(ssoUser.Id);
		}
	}
}

