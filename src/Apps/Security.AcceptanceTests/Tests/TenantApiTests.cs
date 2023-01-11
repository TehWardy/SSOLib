using B2B.AcceptanceTests.Masterdata.Clients;
using Security.Objects.Entities;
using System;
using Tynamix.ObjectFiller;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    [Collection(nameof(TenantTestCollection))]
    public partial class TenantApiTests
    {
        private readonly TenantApiClient client;

        public TenantApiTests(TenantApiClient client)
        {
            this.client = client;
        }

        static int GetRandomNumber()
            => new Random().Next(1, 10);

        static Tenant RandomTenant()
            => GetTenantFiller().Create();

        static Filler<Tenant> GetTenantFiller()
        {
            var filler = new Filler<Tenant>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow)
                .OnProperty(i => i.Roles).IgnoreIt()
                .OnProperty(i => i.UserEvents).IgnoreIt()
                .OnProperty(i => i.Analysis).IgnoreIt();

            return filler;
        }

        static Tenant UpdateTenant(Tenant tenant)
        {
            tenant.Name = new MnemonicString().GetValue();
            tenant.Description = new MnemonicString().GetValue();
            return tenant;
        }
    }
}