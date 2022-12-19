using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Security.Data.EF;
using System.Net.Http;
using SecuritySQLite;
using SSO.AcceptanceTests;
using SharedObjects.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Security.Objects.Entities;
using System.Threading.Tasks;
using Security.Objects.DTOs;
using System.Text;
using System.Collections.Generic;

namespace Security.AcceptanceTests.Clients
{
	public class SSOUserApiClient
	{
        readonly WebApplicationFactory<Program> webApplicationFactory;
        HttpClient api;
        public SSODbContext Database { get; set; }

        const string Endpoint = "Api/Security/SSOUser/";

        public SSOUserApiClient()
        {
            webApplicationFactory = new();
            webApplicationFactory.EnsureSSOSetupForTesting();

            api = webApplicationFactory.CreateClient();
            api.Authenticate("TestUser@corporatelinx.com", "TestPass01!").AsTask().Wait();

            using var scope = webApplicationFactory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            Database = new SSODbContext(
                scopedServices.GetRequiredService<IConfiguration>(),
                scopedServices.GetRequiredService<ISecurityModelBuildProvider>());
        }

        public async ValueTask<Token> LoginAsync(Auth auth, string query = "")
        {
            var content = new StringContent(auth.ToJson(), Encoding.UTF8, "application/json");
            var request = await api.PostAsync("/Api/Account/Login" + query, content);
            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsAsync<Token>();
        }

        public void AddBearerAuthentication(string bearer)
        {
            if (bearer == null)
                api.DefaultRequestHeaders.Authorization = null;
            else
                api.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", bearer);
        }

        public void AddBasicAuthentication(Auth auth)
        {
            if (auth == null)
                api.DefaultRequestHeaders.Authorization = null;
            else
            {
                string encoded = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(auth.User + ":" + auth.Pass));

                api.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("basic", encoded);
            }
        }

        public async ValueTask<IEnumerable<SSOUser>> GetAllSSOUsersAsync(string query = "")
            => await api.GetODataCollection<SSOUser>(Endpoint + query);

        public async ValueTask<SSOUser> Me(string query = "")
            => await api.GetAsync<SSOUser>(Endpoint + "Me()" + query);
    }
}

