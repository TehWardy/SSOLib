namespace Security.Data.Brokers.Storage.Interfaces
{
    public interface ISessionBroker
    {
        ValueTask<Objects.Entities.Session> AddSessionAsync(Objects.Entities.Session Session);
        ValueTask DeleteSessionAsync(Objects.Entities.Session Session);
        IQueryable<Objects.Entities.Session> GetAllSessions();
        ValueTask<Objects.Entities.Session> UpdateSessionAsync(Objects.Entities.Session Session);
    }
}