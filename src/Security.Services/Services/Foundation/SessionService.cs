using Microsoft.AspNetCore.Http;
using Security.Data.Brokers.Serialization;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;

namespace Security.UserManager.Services.Foundation
{
    public class SessionService : ISessionService
    {
        private readonly ISession session;
        private readonly ISerializationBroker serilizationBroker;

        public SessionService(ISession session, ISerializationBroker serilizationBroker)
        {
            this.session = session;
            this.serilizationBroker = serilizationBroker;
        }

        public void SetString(string key, string value)
            => session.SetString(key, value);

        public string GetString(string key)
           => session.GetString(key);

        public SSOUser GetUser()
        {
            var userJson = session.GetString("ssoUser");

            return serilizationBroker.Deserialize<SSOUser>(session.GetString("ssoUser"));
        }

        public void SetUser(SSOUser user)
        { 
            if(user != null)
                session?.SetString("ssoUser", System.Text.Json.JsonSerializer.Serialize(user));
            else
                session?.Remove("ssoUser");
        }

        public void RemoveKey(string key)
            => session.Remove(key);
    }
}