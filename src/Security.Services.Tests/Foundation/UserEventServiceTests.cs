using Moq;
using Security.Data.Brokers.DateTime;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation;
using Security.Services.Foundation.Interfaces;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace Security.Services.Tests.Foundation
{
    public partial class UserEventServiceTests
    {
        private readonly Mock<ISecurityDateTimeOffsetBroker> dateTimeOffsetBrokerMock;
        private readonly Mock<IUserEventBroker> userEventBrokerMock;
        private readonly IUserEventService userEventService;

        public UserEventServiceTests()
        {
            dateTimeOffsetBrokerMock = new Mock<ISecurityDateTimeOffsetBroker>();
            userEventBrokerMock = new Mock<IUserEventBroker>();
            userEventService = new UserEventService(userEventBrokerMock.Object, dateTimeOffsetBrokerMock.Object);
        }

        UserEvent[] RandomUserEvents()
            => Enumerable.Range(1, new Random().Next(10, 20))
                .Select(_ => RandomUserEvent())
                .ToArray();

        UserEvent RandomUserEvent()
            => GetUserEventFiller().Create();

        Filler<UserEvent> GetUserEventFiller()
        {
            var filler = new Filler<UserEvent>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.Now)
                .OnProperty(ue => ue.Session).IgnoreIt()
                .OnProperty(ue => ue.CreatedByUser).IgnoreIt()
                .OnProperty(ue => ue.Tenant).IgnoreIt();

            return filler;
        }
    }
}
