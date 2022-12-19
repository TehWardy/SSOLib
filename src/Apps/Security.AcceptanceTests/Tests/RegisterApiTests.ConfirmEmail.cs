using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
	public partial class RegisterApiTests
	{
        [Fact]
        public async void ConfirmEmailWorksAsExpected()
        {
            //given

            RegisterUser existingRegisterUser = RandomRegisterUser();

            Auth inputAuth = RandomAuth(existingRegisterUser);

            //when
            SSOUser expectedSSOUser = await registerApiClient.RegisterAsync(existingRegisterUser);
            var tokenList = registerApiClient.Database.Tokens
                .AsNoTracking()
                .ToList();

            string userId = expectedSSOUser.Id;
            int reason = (int)TokenUse.Confirmation;

            var token = tokenList
                .First(f => f.UserName == userId && f.Reason == reason);

            expectedSSOUser.EmailConfirmed = true;

            await registerApiClient.PostAsync("ConfirmRegistration?confirmationToken=" + token.Id, null);
            var loginToken = await accountApiClient.LoginAsync(inputAuth);
            ssoUserApiClient.AddBearerAuthentication(loginToken.Id);

            SSOUser actualSSOUser = await ssoUserApiClient.Me();

            //then
            actualSSOUser.Should().BeEquivalentTo(expectedSSOUser);

            await TearDownUserAsync(actualSSOUser.Id);
        }
    }
}

