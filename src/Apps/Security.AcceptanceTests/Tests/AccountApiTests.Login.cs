using Security.Objects.DTOs;
using Security.Objects.Entities;
using System;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class AccountApiTests
    {
        [Fact]
        public async void LoginReturnsTokenAsync()
        {
            //given
            RegisterUser existingRegisterUser = RandomRegisterUser();
            SSOUser existingSSOUser = await registerApiClient.RegisterAsync(existingRegisterUser);

            Auth inputAuth = RandomAuth(existingRegisterUser);

            //when
            Token actualToken = await accountApiClient.LoginAsync(inputAuth);

            //then
            Assert.True(actualToken.UserName == existingSSOUser.Id);
            Assert.True(actualToken.Expires > DateTimeOffset.Now);
            Assert.True(actualToken.Reason == 0);

            await TearDownUserAsync(existingSSOUser.Id);
        }
    }
}
