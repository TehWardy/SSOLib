using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Security.Objects;
using Security.Objects.Entities;

namespace Security.Data.EF
{
	public class IdentitySSODbContext : SSODbContext
	{
        public ISSOAuthInfo AuthInfo { get; }

        bool UserIsPortalAdmin => GetCurrentUser().Roles.Any(r => r.Role.UsersArePortalAdmins);
        SSOUser currentUser;

        public IdentitySSODbContext(IConfiguration configuration, ISSOAuthInfo authInfo, ISecurityModelBuildProvider modelBuildProvider)
            : base(configuration, modelBuildProvider)
        {
            this.AuthInfo = authInfo;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyFilters(modelBuilder);
        }

        void ApplyFilters(ModelBuilder builder)
        {
            builder.Entity<SSOUser>().HasQueryFilter(u => UserIsPortalAdmin || u.Id == AuthInfo.SSOUserId);
            builder.Entity<SSORole>().HasQueryFilter(r => UserIsPortalAdmin || r.Users.Any());
            builder.Entity<SSOUserRole>().HasQueryFilter(ur => ur.User != null);
        }

        public SSOUser GetCurrentUser()
        {
            if (currentUser == null || currentUser.Id != AuthInfo?.SSOUserId)
            {
                var userNameRequested = AuthInfo?.SSOUserId ?? "Guest";
                if (userNameRequested != "Guest")
                    currentUser = Users
                        .IgnoreQueryFilters()
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Id == userNameRequested);

                if (currentUser == null)
                    currentUser = new SSOUser() { Id = "Guest", Roles = Array.Empty<SSOUserRole>() };
            }

            return currentUser;
        }
    }
}

