using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Security.Objects;
using Security.Objects.Entities;

namespace Security.Data.EF
{
    public partial class SSODbContext : DbContext
    {
        public DbSet<SSOUser> Users { get; set; }
        public DbSet<SSORole> Roles { get; set; }

        public DbSet<SSOUserRole> UserRoles { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantAnalysis> TenantAnalysis { get; set; }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }

        readonly IConfiguration configuration;
        readonly ISecurityModelBuildProvider modelBuildProvider;

        public SSODbContext(IConfiguration configuration, ISecurityModelBuildProvider modelBuildProvider)
        {
            this.configuration = configuration;
            this.modelBuildProvider = modelBuildProvider;
        }

        public void Migrate()
            => modelBuildProvider.MigrateDatabase(Database);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuildProvider.Create(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => modelBuildProvider.Configure(configuration, optionsBuilder);

        public IQueryable<UserActivity> GetUserActivity() 
            => UserEvents.IgnoreQueryFilters()
                .Select(ue => new UserActivity
                {
                    // tenant details
                    TenantId = ue.TenantId,
                    TenantName = ue.Tenant.Name,
                    TenantDescription = ue.Tenant.Description,
                    TenantCreatedBy = ue.Tenant.CreatedBy,
                    TenantLastUpdatedBy = ue.Tenant.LastUpdatedBy,
                    TenantCreatedOn = ue.Tenant.CreatedOn,
                    TenantLastUpdated = ue.Tenant.LastUpdated,

                    // user details
                    UserId = ue.CreatedBy,
                    UserDisplayName = ue.CreatedByUser.DisplayName,
                    UserEmail = ue.CreatedByUser.Email,
                    UserPhoneNumber = ue.TenantId,

                    // event details
                    EventId = ue.Id,
                    EventName = ue.EventName,
                    EventValue = ue.Value,
                    EventCreatedOn = ue.CreatedOn,

                    // session details
                    SessionId = ue.SessionId
                });
    }
}