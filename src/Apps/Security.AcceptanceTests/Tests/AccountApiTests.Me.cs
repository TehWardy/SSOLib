using FluentAssertions;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class AccountApiTests
    {
        [Fact]
        public async void MeWorksAsExpectedForBearerToken()
        {
            //given
            userApiClient.UseNoCookiesApiClient();

            RegisterUser existingRegisterUser = RandomRegisterUser();
            SSOUser existingSSOUser = await userApiClient.RegisterAsync(existingRegisterUser);

            Auth inputAuth = RandomAuth(existingRegisterUser);
            Token token = await userApiClient.LoginAsync(inputAuth);

            //when
            userApiClient.AddBearerAuthentication(token.Id);
            SSOUser actualSSOUser = await userApiClient.Me();

            //then
            actualSSOUser.Should().BeEquivalentTo(existingSSOUser);

            await TearDownUserAsync(existingSSOUser.Id);
        }

        [Fact]
        public async void MeWorksAsExpectedForSession()
        {
            //given
            RegisterUser existingRegisterUser = RandomRegisterUser();
            SSOUser existingSSOUser = await userApiClient.RegisterAsync(existingRegisterUser);

            Auth inputAuth = RandomAuth(existingRegisterUser);
            Token token = await userApiClient.LoginAsync(inputAuth);

            //when
            SSOUser actualSSOUser = await userApiClient.Me();

            //then
            actualSSOUser.Should().BeEquivalentTo(existingSSOUser);

            await TearDownUserAsync(existingSSOUser.Id);
        }

        [Fact]
        public async void MeWorksAsExpectedForBasic()
        {
            //given
            userApiClient.UseNoCookiesApiClient();

            RegisterUser existingRegisterUser = RandomRegisterUser();
            SSOUser existingSSOUser = await userApiClient.RegisterAsync(existingRegisterUser);

            Auth inputAuth = RandomAuth(existingRegisterUser);

            //when
            userApiClient.AddBasicAuthentication(inputAuth);
            SSOUser actualSSOUser = await userApiClient.Me();

            //then
            actualSSOUser.Should().BeEquivalentTo(existingSSOUser);

            await TearDownUserAsync(existingSSOUser.Id);
        }
    }
}
