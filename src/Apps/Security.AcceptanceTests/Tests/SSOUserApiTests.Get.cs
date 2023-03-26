using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Security.Objects.Entities;
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
			actualSSOUsers.Should().Contain(expectedSSOUsers);

			foreach (var ssoUser in expectedSSOUsers)
				await registerApiClient.TearDown(ssoUser.Id);
		}
	}
}

