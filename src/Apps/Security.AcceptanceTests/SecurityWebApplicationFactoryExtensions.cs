using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.EF;
using Security.Objects;
using Security.Objects.Entities;
using System.Linq;

namespace SSO.AcceptanceTests
{
    public static class SecurityWebApplicationFactoryExtensions
    {
        static readonly object multiThreadedLock = new();

        public static void EnsureSSOSetupForTesting<TProgram>(this WebApplicationFactory<TProgram> appFactory)
            where TProgram : class
        {
            lock (multiThreadedLock)
            {
                using var scope = appFactory.Services.CreateScope();
                var scopedServices = scope.ServiceProvider;

                using var db = new SSODbContext(
                    scopedServices.GetRequiredService<IConfiguration>(),
                    scopedServices.GetRequiredService<ISSOAuthInfo>(),
                    scopedServices.GetRequiredService<ISecurityModelBuildProvider>());
                db.Database.EnsureCreated();
                SeedTestData(db);
            }
        }

        static void SeedTestData(SSODbContext db)
        {
            SetupTestUser(db);
        }

        static void SetupTestUser(SSODbContext db)
        {
            if (!db.Users.IgnoreQueryFilters().Any(u => u.Id == "TestUser"))
            {
                var allPrivs = db.GetPrivileges().Select(p => p.Id).ToArray();
                var user = db.Add(CreateTestUser()).Entity;
                var role = db.Add(CreateTestAdminsRole(allPrivs)).Entity;
                db.SaveChanges();
                db.Add(new SSOUserRole { UserId = user.Id, RoleId = role.Id });
                db.SaveChanges();
            }
        }

        static SSOUser CreateTestUser()
            => new()
            {
                Id = "TestUser",
                DisplayName = "Test User"
            };

        static SSORole CreateTestAdminsRole(string[] allPrivs)
            => new()
            {
                Name = "Test Admins",
                Privs = string.Join(",",allPrivs),
                UsersArePortalAdmins = true
            };
    }
}
