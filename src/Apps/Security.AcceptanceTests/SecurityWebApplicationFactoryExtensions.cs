using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.Brokers.Encryption;
using Security.Data.EF;
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
                var encryptionBroker = scopedServices.GetService<IPasswordEncryptionBroker>();

                using var db = new SSODbContext(
                    scopedServices.GetRequiredService<IConfiguration>(),
                    null,
                    scopedServices.GetRequiredService<ISecurityModelBuildProvider>());
                db.Database.EnsureCreated();
                SeedTestData(db, encryptionBroker);
            }
        }

        static void SeedTestData(SSODbContext db, IPasswordEncryptionBroker encryptionBroker)
        {
            SetupTestUser(db, encryptionBroker);
        }

        static void SetupTestUser(SSODbContext db, IPasswordEncryptionBroker encryptionBroker)
        {
            if (!db.Users.IgnoreQueryFilters().Any(u => u.Id == "TestUser"))
            {
                var allPrivs = db.GetPrivileges().Select(p => p.Id).ToArray();
                var user = db.Add(CreateTestUser(encryptionBroker)).Entity;
                var role = db.Add(CreateTestAdminsRole(allPrivs)).Entity;
                db.SaveChanges();
                db.Add(new SSOUserRole { UserId = user.Id, RoleId = role.Id });
                db.SaveChanges();
            }
        }

        static SSOUser CreateTestUser(IPasswordEncryptionBroker encryptionBroker)
            => new()
            {
                Id = "TestUser",
                DisplayName = "Test User",
                PasswordHash = encryptionBroker.Encrypt("TestPass01!"),
                AccessFailedCount = 0,
                Email = "TestUser@corporatelinx.com",
                EmailConfirmed = true,
                PhoneNumber = string.Empty,
                PhoneNumberConfirmed = false,
                LockoutEnabled = false,
                LockoutEndDateUtc = null,
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
