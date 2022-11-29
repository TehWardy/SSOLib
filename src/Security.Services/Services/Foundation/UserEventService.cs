using Security.Data.Brokers.DateTime;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Services.Foundation
{
    public class UserEventService : IUserEventService
    {
        private readonly IUserEventBroker broker;
        private readonly ISecurityDateTimeOffsetBroker dateTimeOffsetBroker;

        public UserEventService(IUserEventBroker broker, ISecurityDateTimeOffsetBroker dateTimeOffsetBroker)
        {
            this.broker = broker;
            this.dateTimeOffsetBroker = dateTimeOffsetBroker;
        }

        public async ValueTask<UserEvent> AddUserEventAsync(UserEvent userEvent)
        {
            userEvent.CreatedOn = dateTimeOffsetBroker.GetCurrentTime();
            return await broker.AddUserEventAsync(userEvent);
        }

        public ValueTask DeleteUserEventAsync(UserEvent userEvent)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserEvent> GetAllUserEvents()
        {
            throw new NotImplementedException();
        }

        public ValueTask<UserEvent> UpdateUserEventAsync(UserEvent userEvent)
        {
            throw new NotImplementedException();
        }
    }
}
