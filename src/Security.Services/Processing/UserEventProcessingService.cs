﻿using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Processing
{
    public class UserEventProcessingService : IUserEventProcessingService
    {
        private readonly IUserEventService userEventService;

        public UserEventProcessingService(IUserEventService userEventService)
        {
            this.userEventService = userEventService;
        }

        public ValueTask<UserEvent> AddUserEventAsync(UserEvent userEvent)
            => userEventService.AddUserEventAsync(userEvent);

        public ValueTask DeleteUserEventAsync(UserEvent userEvent)
            => userEventService.DeleteUserEventAsync(userEvent);

        public IQueryable<UserEvent> GetAllUserEvents()
            => userEventService.GetAllUserEvents();
    }
}
