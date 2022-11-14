using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage.Interfaces
{
    public interface ISSOUserBroker
    {
        ValueTask<SSOUser> AddSSOUserAsync(SSOUser user);
        ValueTask DeleteSSOUserAsync(SSOUser SSOUser);
        IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false);
        ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser user);
    }
}