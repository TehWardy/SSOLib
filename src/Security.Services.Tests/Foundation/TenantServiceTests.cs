using Moq;
using Security.Data.Brokers.DateTime;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation;
using Security.Services.Services.Foundation.Interfaces;
using System;
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
