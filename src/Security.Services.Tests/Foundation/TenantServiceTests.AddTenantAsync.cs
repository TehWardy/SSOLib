using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using System;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class TenantServiceTests
    {
        [Fact]
        public async void AddTenantAsyncWorksAsExpected()
        {
            //given
            Tenant inputTenant = RandomTenant();
            Tenant expectedTenant = inputTenant.DeepClone();
            DateTimeOffset expectedTime = DateTimeOffset.Now;

            dateTimeOffsetBrokerMock.Setup(dateTimeOffsetBrokerMock =>
                dateTimeOffsetBrokerMock.GetCurrentTime())
                .Returns(expectedTime);

            tenantBrokerMock.Setup(tenantBrokerMock =>
                tenantBrokerMock.AddTenantAsync(inputTenant))
                .ReturnsAsync(inputTenant);

            expectedTenant.CreatedOn = expectedTime;
            expectedTenant.LastUpdated = expectedTime;

            //when
            Tenant actualTenant = await tenantService.AddTenantAsync(inputTenant);

            //then
            actualTenant.Should().BeEquivalentTo(expectedTenant);

            tenantBrokerMock.Verify(tenantBrokerMock =>
                tenantBrokerMock.AddTenantAsync(inputTenant), Times.Once());

            tenantBrokerMock.VerifyNoOtherCalls();
        }
    }
}
