using Security.Objects.Entities;
using System;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class TenantApiTests
    {
        [Fact]
        public async void ShouldDeleteTenants()
        {
            // given
            Tenant inputTenant = await client.AddTenantAsync(RandomTenant());

            // when 
            await client.DeleteTenantAsync(inputTenant.Id);

            // then
            await Assert.ThrowsAsync<Exception>(async () => await client.GetTenantById(inputTenant.Id));
        }
    }
}
