using Security.Data.Brokers.Storage.Interfaces;
using Security.Data.EF.Interfaces;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage
{
    public class TenantAnalysisBroker : ITenantAnalysisBroker
    {
        ISSODbContextFactory contextFactory;

        public TenantAnalysisBroker(ISSODbContextFactory contextFactory)
            => this.contextFactory = contextFactory;

        public async ValueTask<TenantAnalysis> AddTenantAnalysisAsync(TenantAnalysis tenantAnalysis)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = await context.TenantAnalysis.AddAsync(tenantAnalysis);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask<TenantAnalysis> UpdateTenantAnalysisAsync(TenantAnalysis tenantAnalysis)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.TenantAnalysis.Update(tenantAnalysis);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask DeleteTenantAnalysisAsync(TenantAnalysis tenantAnalysis)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.TenantAnalysis.Remove(tenantAnalysis);
            await context.SaveChangesAsync();
        }

        public IQueryable<TenantAnalysis> GetAllTenantAnalysiss()
        {
            var context = contextFactory.CreateDbContext();
            return context.TenantAnalysis;
        }
    }
}