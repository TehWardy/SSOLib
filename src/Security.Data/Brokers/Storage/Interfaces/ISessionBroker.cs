using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage.Interfaces
{
    public interface ISessionBroker
    {
        ValueTask<Session> AddSessionAsync(Session Session);
        ValueTask DeleteSessionAsync(Session Session);
        IQueryable<Session> GetAllSessions();
        ValueTask<Session> UpdateSessionAsync(Session Session);
    }
}