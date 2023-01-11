using FluentAssertions;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class TenantApiTests
    {
        [Fact]
        public async void ShouldGetTenantsById()
        {
            // given
            Tenant inputTenant = RandomTenant();
            Tenant expectedTenant = await client.AddTenantAsync(inputTenant);

            // when
            var actualTenant = await client.GetTenantById(expectedTenant.Id);

            // then
            actualTenant.Should().BeEquivalentTo(expectedTenant);
            await client.DeleteTenantAsync(actualTenant.Id);
        }
    }
}
