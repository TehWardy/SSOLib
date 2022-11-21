using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;

namespace Security.UserManager.Services.Processing
{
    public class SessionProcessingService : ISessionProcessingService
    {
        private readonly ISessionService sessionService;

        public SessionProcessingService(ISessionService sessionService)
            => this.sessionService = sessionService;

        public string GetString(string key)
            => sessionService.GetString(key);

        public SSOUser GetUser() 
            => sessionService.GetUser();

        public void SetString(string key, string value)
            => sessionService.SetString(key, value);

        public void SetUser(SSOUser user)
            => sessionService.SetUser(user);
    }
}