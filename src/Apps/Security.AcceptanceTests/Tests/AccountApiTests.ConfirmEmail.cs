using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
	public partial class AccountApiTests
	{
        [Fact]
        public async void ConfirmEmailWorksAsExpected()
        {
            //given

            RegisterUser existingRegisterUser = RandomRegisterUser();

            Auth inputAuth = RandomAuth(existingRegisterUser);

            //when
            SSOUser expectedSSOUser = await userApiClient.RegisterAsync(existingRegisterUser);
            var tokenList = userApiClient.Database.Tokens
                .AsNoTracking()
                .ToList();

            string userId = expectedSSOUser.Id;
            int reason = (int)TokenUse.Confirmation;

            var token = tokenList
                .First(f => f.UserName == userId && f.Reason == reason);

            expectedSSOUser.EmailConfirmed = true;

            await userApiClient.PostAsync("ConfirmRegistration?confirmationToken=" + token.Id, null);
            var loginToken = await userApiClient.LoginAsync(inputAuth);
            userApiClient.AddBearerAuthentication(loginToken.Id);

            SSOUser actualSSOUser = await userApiClient.Me();

            //then
            actualSSOUser.Should().BeEquivalentTo(expectedSSOUser);

            await TearDownUserAsync(actualSSOUser.Id);
        }
    }
}

