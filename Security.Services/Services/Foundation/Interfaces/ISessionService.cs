using Security.Objects.Entities;

namespace Security.Services.Services.Foundation.Interfaces
{
    public interface ISessionService
    {
        void SetString(string key, string value);
        string GetString(string key);
        SSOUser GetUser();
        void SetUser(SSOUser user);
    }
}