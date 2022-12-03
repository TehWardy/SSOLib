using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Processing
{
    public partial class UserEventProcessingServiceTests
    {
        [Fact]
        public async void ShouldDeleteUserEventAsync()
        {
            //given
            UserEvent inputUserEvent = RandomUserEvent();

            //when
            await userEventProcessingService.DeleteUserEventAsync(inputUserEvent);

            //then
            userEventServiceMock.Verify(userEventServiceMock =>
                userEventServiceMock.DeleteUserEventAsync(inputUserEvent),
                Times.Once());

            userEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
