using FluentAssertions;
using Force.DeepCloner;
using Security.Objects.Entities;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    public partial class TenantApiTests
    {
        [Fact]
        public async void ShouldCreateTenants()
        {
            // given
            Tenant inputTenant = RandomTenant();
            Tenant expectedTenant = inputTenant.DeepClone();

            // when 
            var actualTenant = await client.AddTenantAsync(inputTenant);
            expectedTenant.Id = actualTenant.Id;
            expectedTenant.CreatedBy = "TestUser";
            expectedTenant.LastUpdatedBy = "TestUser";
            expectedTenant.CreatedOn = actualTenant.CreatedOn;
            expectedTenant.LastUpdated = actualTenant.LastUpdated;

            // then
            actualTenant.Should().BeEquivalentTo(expectedTenant);
            await client.DeleteTenantAsync(actualTenant.Id);
        }
    }
}
