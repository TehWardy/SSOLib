using Moq;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing;
using Security.Services.Services.Processing.Interfaces;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace Security.Services.Tests.Processing
{
    public partial class UserEventProcessingServiceTests
    {
        private readonly Mock<IUserEventService> userEventServiceMock;
        private readonly IUserEventProcessingService userEventProcessingService;

        public UserEventProcessingServiceTests()
        {
            userEventServiceMock = new Mock<IUserEventService>();
            userEventProcessingService = new UserEventProcessingService(userEventServiceMock.Object);
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
