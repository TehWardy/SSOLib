using FluentAssertions;
using Force.DeepCloner;
using Security.Objects.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
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

        [Fact]
        public async void ShouldGetAllTenants()
        {
            var randomSetSize = GetRandomNumber();

            // given
            var inputTenants = new List<Tenant>();
            var expectedTenants = new List<Tenant>();

            for (int i = 0; i < randomSetSize; i++)
            {
                var tenant = RandomTenant();
                inputTenants.Add(tenant);
                var newTenant = await client.AddTenantAsync(tenant);
                var copy = tenant.DeepClone();
                copy.Id = newTenant.Id;
                copy.CreatedBy = "TestUser";
                copy.LastUpdatedBy = "TestUser";
                copy.CreatedOn = newTenant.CreatedOn;
                copy.LastUpdated = newTenant.LastUpdated;
                expectedTenants.Add(copy);
            }

            // when
            var actualTenants = await client.GetTenantsAsync();

            // then
            foreach (var expectedTenant in expectedTenants)
            {
                var actualTenant = actualTenants.First(Tenant => Tenant.Id == expectedTenant.Id);
                actualTenant.Should().BeEquivalentTo(expectedTenant);
                await client.DeleteTenantAsync(actualTenant.Id);
            }
        }
    }
}
