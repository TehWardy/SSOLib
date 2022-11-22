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
        {
            if (sessionService.GetString("ssoUser") == null)
                return null;

            return sessionService.GetUser();
        }

        public void SetString(string key, string value)
            => sessionService.SetString(key, value);

        public void SetUser(SSOUser user)
        {
            if (sessionService.GetString("ssoUser") != null)
                sessionService.RemoveKey("ssoUser");

            sessionService.SetUser(user);
        }

        public void Remove(string key)
            => sessionService.RemoveKey(key);

        public void Logout()
        {
            if (sessionService.GetString("ssoUser") != null)
                sessionService.RemoveKey("ssoUser");
        }
    }
}