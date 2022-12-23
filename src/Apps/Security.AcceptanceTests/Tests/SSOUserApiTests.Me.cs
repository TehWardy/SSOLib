using System.Net;
using FluentAssertions;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class SSOUserApiTests
    {
        [Fact]
        public async void MeWorksAsExpectedForBearerToken()
        {
            //given
            var client = ssoUserApiClient.UseNoCookiesApiClient();

            RegisterUser existingRegisterUser = RandomRegisterUser();
            SSOUser existingSSOUser = await registerApiClient.RegisterAsync(existingRegisterUser);

            Auth inputAuth = RandomAuth(existingRegisterUser);
            Token token = await accountApiClient.LoginAsync(inputAuth);

            //when
            ssoUserApiClient.AddBearerAuthentication(client, token.Id);
            SSOUser actualSSOUser = await ssoUserApiClient.Me(client);

            //then
            actualSSOUser.Should().BeEquivalentTo(existingSSOUser);

            await TearDownUserAsync(existingSSOUser.Id);
        }

        [Fact]
        public async void MeWorksAsExpectedForSession()
        {
            //given
            var client = ssoUserApiClient.GetClient();

            RegisterUser existingRegisterUser = RandomRegisterUser();
            SSOUser existingSSOUser = await registerApiClient.RegisterAsync(existingRegisterUser);

            Auth inputAuth = RandomAuth(existingRegisterUser);
            Token token = await ssoUserApiClient.LoginAsync(client, inputAuth);

            //when
            SSOUser actualSSOUser = await ssoUserApiClient.Me(client);

            //then
            actualSSOUser.Should().BeEquivalentTo(existingSSOUser);

            await TearDownUserAsync(existingSSOUser.Id);
        }

        [Fact]
        public async void MeWorksAsExpectedForBasic()
        {
            //given
            var client = ssoUserApiClient.UseNoCookiesApiClient();

            RegisterUser existingRegisterUser = RandomRegisterUser();
            SSOUser existingSSOUser = await registerApiClient.RegisterAsync(existingRegisterUser);

            Auth inputAuth = RandomAuth(existingRegisterUser);

            //when
            ssoUserApiClient.AddBasicAuthentication(client, inputAuth);
            SSOUser actualSSOUser = await ssoUserApiClient.Me(client);

            //then
            actualSSOUser.Should().BeEquivalentTo(existingSSOUser);

            await TearDownUserAsync(existingSSOUser.Id);
        }
    }
}
