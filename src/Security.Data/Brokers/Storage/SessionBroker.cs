using Security.Data.Brokers.Storage.Interfaces;
using Security.Data.EF.Interfaces;

namespace Security.Data.Brokers.Storage
{
    public class SessionBroker : ISessionBroker
    {
        ISSODbContextFactory contextFactory;

        public SessionBroker(ISSODbContextFactory contextFactory)
            => this.contextFactory = contextFactory;

        public async ValueTask<Objects.Entities.Session> AddSessionAsync(Objects.Entities.Session Session)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = await context.Sessions.AddAsync(Session);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask<Objects.Entities.Session> UpdateSessionAsync(Objects.Entities.Session Session)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.Sessions.Update(Session);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask DeleteSessionAsync(Objects.Entities.Session Session)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.Sessions.Remove(Session);
            await context.SaveChangesAsync();
        }

        public IQueryable<Objects.Entities.Session> GetAllSessions()
        {
            var context = contextFactory.CreateDbContext();
            return context.Sessions;
        }
    }
}
