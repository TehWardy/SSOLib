using FluentAssertions;
using Moq;
using Security.Objects.Entities;
using System.Linq;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class TenantServiceTests
    {
        [Fact]
        public void GetAllTenantsWorksAsExpected()
        {
            //given
            IQueryable<Tenant> expectedTenants = RandomTenants()
                .AsQueryable();

            tenantBrokerMock.Setup(tenantBrokerMock =>
                tenantBrokerMock.GetAllTenants())
                .Returns(expectedTenants);

            //when
            IQueryable<Tenant> actualTenants = tenantService.GetAllTenants();

            //then
            actualTenants.Should().BeEquivalentTo(expectedTenants);

            tenantBrokerMock.Verify(tenantBrokerMock =>
                tenantBrokerMock.GetAllTenants(),
                Times.Once());

            tenantBrokerMock.VerifyNoOtherCalls();
        }
    }
}
