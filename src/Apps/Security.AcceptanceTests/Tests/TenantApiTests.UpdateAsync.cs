using FluentAssertions;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class TenantApiTests
    {
        [Fact]
        public async void ShouldUpdateTenants()
        {
            // given
            Tenant randomTenant = await client.AddTenantAsync(RandomTenant());
            Tenant modifiedTenant = UpdateTenant(randomTenant);

            // when
            Tenant actualTenant = await client.UpdateTenantAsync(modifiedTenant);
            Tenant actualTenantAgain = await client.GetTenantById(randomTenant.Id);
            modifiedTenant.LastUpdated = actualTenant.LastUpdated;

            // then
            actualTenant.Should().BeEquivalentTo(modifiedTenant);
            actualTenantAgain.Should().BeEquivalentTo(modifiedTenant);
            await client.DeleteTenantAsync(actualTenant.Id);
        }
    }
}
