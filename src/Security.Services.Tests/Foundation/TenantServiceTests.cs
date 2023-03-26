using Moq;
using Security.Data.Brokers.DateTime;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Foundation;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace Security.Services.Tests.Foundation
{
    public partial class TenantServiceTests
    {
        private readonly Mock<ITenantBroker> tenantBrokerMock;
        private readonly Mock<ISecurityDateTimeOffsetBroker> dateTimeOffsetBrokerMock;
        private readonly ITenantService tenantService;

        public TenantServiceTests()
        {
            tenantBrokerMock = new Mock<ITenantBroker>();
            dateTimeOffsetBrokerMock = new Mock<ISecurityDateTimeOffsetBroker>();
            tenantService = new TenantService(tenantBrokerMock.Object, dateTimeOffsetBrokerMock.Object);
        }

        Tenant[] RandomTenants()
            => Enumerable.Range(1, new Random().Next(10, 20))
                .Select(_ => RandomTenant())
                .ToArray();

        Tenant RandomTenant()
            => GetTenantFiller().Create();

        Filler<Tenant> GetTenantFiller()
        {
            var filler = new Filler<Tenant>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.Now)
                .OnProperty(t => t.Analysis).IgnoreIt()
                .OnProperty(t => t.UserEvents).IgnoreIt()
                .OnProperty(t => t.Roles).IgnoreIt();

            return filler;
        }
    }
}
