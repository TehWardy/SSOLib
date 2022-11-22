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
            Token token = await authenticationApiClient.LoginAsync(inputAuth);

            //when
            userApiClient.AddBearerAuthentication(token.Id);
            SSOUser actualSSOUser = await userApiClient.Me();

            //then
            actualSSOUser.Should().BeEquivalentTo(existingSSOUser);

            await TearDownUserAsync(existingSSOUser.Id);
        }
    }
}
