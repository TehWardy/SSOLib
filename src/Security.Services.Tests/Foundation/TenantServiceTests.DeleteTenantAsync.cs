using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class TenantServiceTests
    {
        [Fact]
        public async void DeleteTenantAsyncWorksAsExpected()
        {
            //given
            Tenant inputTenant = RandomTenant();

            //when
            await tenantService.DeleteTenantAsync(inputTenant);

            //then
            tenantBrokerMock.Verify(tenantBrokerMock =>
                tenantBrokerMock.DeleteTenantAsync(inputTenant),
                Times.Once());

            tenantBrokerMock.VerifyNoOtherCalls();
        }
    }
}
