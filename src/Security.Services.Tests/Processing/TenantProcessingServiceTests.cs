using Moq;
using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing;
using Security.Services.Processing.Interfaces;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace Security.Services.Tests.Processing
{
    public partial class TenantProcessingServiceTests
    {
        private readonly Mock<ITenantService> tenantServiceMock;
        private readonly ITenantProcessingService tenantProcessingService;

        public TenantProcessingServiceTests()
        {
            tenantServiceMock = new Mock<ITenantService>();
            tenantProcessingService = new TenantProcessingService(tenantServiceMock.Object);
        }

        public Tenant[] RandomTenants()
            => Enumerable.Range(1, new Random().Next(1, 20))
                .Select(_ => RandomTenant())
                .ToArray();

        public Tenant RandomTenant()
            => GetTenantFiller().Create();

        public Filler<Tenant> GetTenantFiller()
        {
            var filler = new Filler<Tenant>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.Now)
                .OnProperty(p => p.Analysis).IgnoreIt()
                .OnProperty(p => p.UserEvents).IgnoreIt()
                .OnProperty(p => p.Roles).IgnoreIt();

            return filler;
        } 
    }
}
