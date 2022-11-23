using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class SSOUserServiceTests
    {
        [Fact]
        public async void ShouldAddSSOUserAsync()
        {
            // given
            SSOUser inputSSOUser = RandomUser(RandomString());
            SSOUser expectedSSOUser = inputSSOUser.DeepClone();

            userBrokerMock.Setup(broker => broker.AddSSOUserAsync(inputSSOUser)).ReturnsAsync(expectedSSOUser);

            // when
            SSOUser actualSSOUser = await userService.AddSSOUserAsync(inputSSOUser);

            // then
            actualSSOUser.Should().BeEquivalentTo(expectedSSOUser);

            userBrokerMock.Verify(broker => broker.AddSSOUserAsync(inputSSOUser), Times.Once);
            userBrokerMock.VerifyNoOtherCalls();
        }
    }
}

