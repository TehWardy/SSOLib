using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class SSOUserServiceTests
    {
        [Fact]
        public async void ShouldDeleteSSOUserAsync()
        {
            // given
            SSOUser inputSSOUser = RandomUser(RandomString());
            SSOUser expectedSSOUser = inputSSOUser.DeepClone();

            // when
            await userService.DeleteSSOUserAsync(inputSSOUser);

            // then
            userBrokerMock.Verify(broker => 
                broker.DeleteSSOUserAsync(inputSSOUser), 
                Times.Once);

            userBrokerMock.VerifyNoOtherCalls();
        }
    }
}