using Security.Data.Brokers.Storage.Interfaces;
using Security.Data.EF.Interfaces;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage
{
    public class SSOUserRoleBroker : ISSOUserRoleBroker
    {
        ISSODbContextFactory contextFactory;

        public SSOUserRoleBroker(ISSODbContextFactory contextFactory)
            => this.contextFactory = contextFactory;

        public async ValueTask<SSOUserRole> AddSSOUserRoleAsync(SSOUserRole userRole)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = await context.UserRoles.AddAsync(userRole);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask DeleteSSOUserRoleAsync(SSOUserRole userRole)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.UserRoles.Remove(userRole);
            await context.SaveChangesAsync();
        }

        public IQueryable<SSOUserRole> GetAllSSOUserRoles()
        {
            var context = contextFactory.CreateDbContext();
            return context.UserRoles;
        }
    }
}