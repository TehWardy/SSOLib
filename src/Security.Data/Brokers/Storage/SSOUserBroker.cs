using Microsoft.EntityFrameworkCore;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Data.EF.Interfaces;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage
{
    public class SSOUserBroker : ISSOUserBroker
    {
        ISSODbContextFactory contextFactory;

        public SSOUserBroker(ISSODbContextFactory contextFactory)
            => this.contextFactory = contextFactory;

        public async ValueTask<SSOUser> AddSSOUserAsync(SSOUser user)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser user)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.Users.Update(user);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask DeleteSSOUserAsync(SSOUser SSOUser)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.Users.Remove(SSOUser);
            await context.SaveChangesAsync();
        }

        public IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false)
        {
            var context = contextFactory.CreateDbContext();

            return ignoreFilters
                ? context.Users.IgnoreQueryFilters()
                : context.Users;
        }
    }
}