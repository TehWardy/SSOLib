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

        // unknown as of yet if this should be a thing
        //public DbSet<SSOPrivilege> Privileges { get; set; }
        public DbSet<SSOUserRole> UserRoles { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantAnalysis> TenantAnalysis { get; set; }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }

        public ISSOAuthInfo AuthInfo { get; }

        bool UserIsPortalAdmin => GetCurrentUser().Roles.Any(r => r.Role.UsersArePortalAdmins);

        readonly IConfiguration configuration;
        readonly ISecurityModelBuildProvider modelBuildProvider;
        SSOUser currentUser;

        public SSODbContext(IConfiguration configuration, ISSOAuthInfo authInfo, ISecurityModelBuildProvider modelBuildProvider)
        {
            this.configuration = configuration;
            this.AuthInfo = authInfo;
            this.modelBuildProvider = modelBuildProvider;
        }

        public void Migrate()
            => modelBuildProvider.MigrateDatabase(Database);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuildProvider.Create(modelBuilder);
            ApplyFilters(modelBuilder);
        }

        void ApplyFilters(ModelBuilder builder)
        {
            builder.Entity<SSOUser>().HasQueryFilter(u => UserIsPortalAdmin || u.Id == AuthInfo.SSOUserId);
            builder.Entity<SSORole>().HasQueryFilter(r => UserIsPortalAdmin || r.Users.Any());
            builder.Entity<SSOUserRole>().HasQueryFilter(ur => ur.User != null);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            modelBuildProvider.Configure(configuration, optionsBuilder);
        }

        public SSOUser GetCurrentUser()
        {
            if (currentUser == null || currentUser.Id != AuthInfo?.SSOUserId)
            {
                var userNameRequested = AuthInfo?.SSOUserId ?? "Guest";
                if (userNameRequested != "Guest")
                    currentUser = Users
                        .IgnoreQueryFilters()
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Id == userNameRequested);

                if (currentUser == null)
                    currentUser = new SSOUser() { Id = "Guest", Roles = Array.Empty<SSOUserRole>() };
            }

            return currentUser;
        }

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