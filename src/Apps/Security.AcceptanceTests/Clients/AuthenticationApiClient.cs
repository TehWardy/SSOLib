using Microsoft.AspNetCore.Mvc.Testing;
using Security.Data.EF;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using SecuritySQLite;
using SharedObjects.Extensions;
using SSO.AcceptanceTests;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Security.AcceptanceTests.Clients
{
    public class AuthenticationApiClient
    {
        readonly WebApplicationFactory<Program> webApplicationFactory;
        readonly HttpClient Api;

        public SSODbContext Database { get; private set; }

        const string Endpoint = "Api/Account/";

        public AuthenticationApiClient()
        {
            webApplicationFactory = new();
            webApplicationFactory.EnsureSSOSetupForTesting();

            Api = webApplicationFactory.CreateClient();
            Api.Authenticate("TestUser", "TestPass01!").AsTask().Wait();
        }

        public async ValueTask<Token> LoginAsync(Auth auth, string query = "")
        {
            var content = new StringContent(auth.ToJson(), Encoding.UTF8, "application/json");
            var request = await Api.PostAsync(Endpoint + "Login" + query, content);
            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsAsync<Token>();
        }
    }
}
