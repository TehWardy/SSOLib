using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.EF;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using SecuritySQLite;
using SharedObjects.Extensions;
using SSO.AcceptanceTests;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

            Database = new SSODbContext(scopedServices.GetRequiredService<ISecurityModelBuildProvider>());
        }

        public async ValueTask<Token> LoginAsync(Auth auth, string query = "", bool keepSessionCookie = false)
        {
            var content = new StringContent(auth.ToJson(), Encoding.UTF8, "application/json");
            var request = await api.PostAsync("/Api/Account/Login" + query, content);
            request.EnsureSuccessStatusCode();

            if (keepSessionCookie)
            {
                var cookie = request.RequestMessage.Headers.GetValues("Cookie").First();
                api.DefaultRequestHeaders.Append(new KeyValuePair<string, IEnumerable<string>>(".AspNetSession", new[] { cookie }));
            }

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

