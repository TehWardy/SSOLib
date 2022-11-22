using Security.Objects.Entities;

namespace Security.Services.Services.Processing.Interfaces
{
    public interface ISessionProcessingService
    {
        void SetString(string key, string value);
        string GetString(string key);
        SSOUser GetUser();
        void SetUser(SSOUser user);
        void Logout();
    }
}
