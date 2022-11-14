using Microsoft.AspNetCore.Http;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;

namespace Security.UserManager.Services.Foundation
{
    public class SessionService : ISessionService
    {
        readonly ISession session;

        public SessionService(ISession session)
            => this.session = session;

        public void SetString(string key, string value)
            => session.SetString(key, value);

        public string GetString(string key)
           => session.GetString(key);

        public SSOUser GetUser()
        {
            var userJson = session.GetString("ssoUser");

            return userJson != null
                ? System.Text.Json.JsonSerializer.Deserialize<SSOUser>(userJson)
                : new SSOUser { Id = "Guest", DisplayName = "Guest" };
        }

        public void SetUser(SSOUser user)
        { 
            if(user != null)
                session?.SetString("ssoUser", System.Text.Json.JsonSerializer.Serialize(user));
            else
                session?.Remove("ssoUser");
        }
    }
}