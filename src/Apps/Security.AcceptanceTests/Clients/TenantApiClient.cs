using Microsoft.AspNetCore.Mvc.Testing;
using Security.AcceptanceTests.Exceptions;
using Security.Data.EF;
using Security.Objects.Entities;
using SecuritySQLite;
using SharedObjects.Extensions;
using SSO.AcceptanceTests;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace B2B.AcceptanceTests.Masterdata.Clients
{
    public partial class TenantApiClient
    {
        readonly WebApplicationFactory<Program> webApplicationFactory;
        readonly HttpClient Api;

        public SSODbContext Database { get; private set; }

        const string Endpoint = "Api/Security/Tenant";

        public TenantApiClient()
        {
            webApplicationFactory = new();
            webApplicationFactory.EnsureSSOSetupForTesting();

            Api = webApplicationFactory.CreateClient();
            Api.Authenticate("TestUser", "TestPass01!").AsTask().Wait();
        }

        public async ValueTask<IEnumerable<Tenant>> GetTenantsAsync()
            => await Api.GetODataCollection<Tenant>(Endpoint);

        public async ValueTask<Tenant> GetTenantById(string id, string query = "")
            => await Api.GetAsync<Tenant>($"{Endpoint}('{id}'){query}");

        public async ValueTask<Tenant> AddTenantAsync(Tenant tenant)
            => await Api.AddAsync(Endpoint, tenant);
        public async ValueTask<Tenant> UpdateTenantAsync(Tenant tenant)
            => await Api.UpdateAsync($"{Endpoint}('{tenant.Id}')", tenant);

        public async ValueTask DeleteTenantAsync(string id)
        {
            var response = await Api.DeleteAsync($"{Endpoint}('{id}')");
            if ((int)response.StatusCode == 500)
                throw new InternalServerErrorException(await response.Content.ReadAsStringAsync());

            if ((int)response.StatusCode == 400)
                throw new BadRequestException(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
        }
    }
}