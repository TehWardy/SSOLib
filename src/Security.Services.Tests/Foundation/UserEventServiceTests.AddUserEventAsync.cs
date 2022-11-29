using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using System;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class UserEventServiceTests
    {
        [Fact]
        public async void AddUserEventAsyncWorksAsExpected()
        {
            //given
            UserEvent inputUserEvent = RandomUserEvent();
            UserEvent expectedUserEvent = inputUserEvent.DeepClone();
            DateTimeOffset expectedTime = DateTimeOffset.Now;

            expectedUserEvent.CreatedOn = expectedTime;

            userEventBrokerMock.Setup(userEventBrokerMock =>
                userEventBrokerMock.AddUserEventAsync(inputUserEvent))
                .ReturnsAsync(inputUserEvent);

            dateTimeOffsetBrokerMock.Setup(dateTimeOffsetBrokerMock =>
                dateTimeOffsetBrokerMock.GetCurrentTime())
                .Returns(expectedTime);

            //when
            UserEvent actualUserEvent = await userEventService.AddUserEventAsync(inputUserEvent);

            //then
            actualUserEvent.Should().BeEquivalentTo(expectedUserEvent);

            userEventBrokerMock.Verify(userEventBrokerMock =>
                userEventBrokerMock.AddUserEventAsync(inputUserEvent),
                Times.Once());

            userEventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
