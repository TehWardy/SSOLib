using FluentAssertions;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class RegisterApiTests
    {
        [Fact]
        public async void ShouldRegisterAccountAsync()
        {
            //given
            RegisterUser inputRegisterUser = RandomRegisterUser();
            SSOUser expectedSSOUser = new SSOUser()
            {
                AccessFailedCount = 0,
                DisplayName = inputRegisterUser.DisplayName,
                Email = inputRegisterUser.Email,
                EmailConfirmed = false,
                LockoutEnabled = false,
                LockoutEndDateUtc = null,
                PhoneNumber = inputRegisterUser.PhoneNumber,
                PhoneNumberConfirmed = false,
            };

            //when
            SSOUser actualSSOUser = await registerApiClient.RegisterAsync(inputRegisterUser);
            expectedSSOUser.Id = actualSSOUser.Id;
            expectedSSOUser.PasswordHash = actualSSOUser.PasswordHash;

            //then
            actualSSOUser.Should().BeEquivalentTo(expectedSSOUser);

            Assert.True(!string.IsNullOrEmpty(actualSSOUser.PasswordHash) && actualSSOUser.PasswordHash.Trim().Length > 5);

            await TearDownUserAsync(actualSSOUser.Id);
        }
    }
}
