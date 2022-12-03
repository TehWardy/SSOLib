using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Security.Services.Tests.Processing
{
    public partial class UserEventProcessingServiceTests
    {
        [Fact]
        public async void AddUserEventAsyncWorksAsExpected()
        {
            //given
            UserEvent inputUserEvent = RandomUserEvent();
            UserEvent expectedUserEvent = inputUserEvent.DeepClone();

            userEventServiceMock.Setup(userEventServiceMock =>
                userEventServiceMock.AddUserEventAsync(inputUserEvent))
                .ReturnsAsync(expectedUserEvent);

            //when
            UserEvent actualUserEvent = await userEventProcessingService.AddUserEventAsync(inputUserEvent);

            //then
            actualUserEvent.Should().BeEquivalentTo(expectedUserEvent);

            userEventServiceMock.Verify(userEventServiceMock =>
                userEventServiceMock.AddUserEventAsync(inputUserEvent),
                Times.Once());

            userEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
