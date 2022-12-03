using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Processing
{
    public partial class TenantProcessingServiceTests
    {
        [Fact]
        public async void ShouldDeleteTenantAsync()
        {
            //given
            Tenant inputTenant = RandomTenant();

            //when
            await tenantProcessingService.DeleteTenantAsync(inputTenant);

            //then
            tenantServiceMock.Verify(tenantServiceMock =>
                tenantServiceMock.DeleteTenantAsync(inputTenant),
                Times.Once());

            tenantServiceMock.VerifyNoOtherCalls();
        }
    }
}
