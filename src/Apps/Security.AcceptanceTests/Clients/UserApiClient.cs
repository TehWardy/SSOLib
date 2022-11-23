using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.AcceptanceTests.Exceptions;
using Security.Data.Brokers.Encryption;
using Security.Data.EF;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using SecuritySQLite;
using SharedObjects.Extensions;
using SSO.AcceptanceTests;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Security.AcceptanceTests.Clients
{
    public class UserApiClient
    {
        readonly WebApplicationFactory<Program> webApplicationFactory;
        HttpClient Api;
        public SSODbContext Database { get; set; }

        const string Endpoint = "Api/Account/";

        public UserApiClient()
        {
            webApplicationFactory = new();
            webApplicationFactory.EnsureSSOSetupForTesting();

            Api = webApplicationFactory.CreateClient();
            Api.Authenticate("TestUser@corporatelinx.com", "TestPass01!").AsTask().Wait();

            using var scope = webApplicationFactory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            Database = new SSODbContext(
                scopedServices.GetRequiredService<IConfiguration>(),
                scopedServices.GetRequiredService<ISecurityModelBuildProvider>());
        }

        public void AddBearerAuthentication(string bearer)
        {
            if (bearer == null)
                Api.DefaultRequestHeaders.Authorization = null;
            else
                Api.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", bearer);
        }

        public void AddBasicAuthentication(Auth auth)
        {
            if (auth == null)
                Api.DefaultRequestHeaders.Authorization = null;
            else
            {
                string encoded = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(auth.User + ":" + auth.Pass));

                Api.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("basic", encoded);
            }
        }

        public HttpClient UseNoCookiesApiClient()
            => Api = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = false });

        public async ValueTask<SSOUser> Me(string query = "")
            => await Api.GetAsync<SSOUser>(Endpoint + "Me" + query);

        public async ValueTask PostAsync(string query, object content)
        {
            var request = await Api.PostAsync(Endpoint + query, new StringContent(content.ToJson(), Encoding.UTF8, "application/json"));

            if ((int)request.StatusCode == 500)
                throw new InternalServerErrorException(await request.Content.ReadAsStringAsync());

            if ((int)request.StatusCode == 400)
                throw new BadRequestException(await request.Content.ReadAsStringAsync());

            request.EnsureSuccessStatusCode();
        }


        public async ValueTask<SSOUser> RegisterAsync(RegisterUser registerUser, string query = "")
        {
            var content = new StringContent(registerUser.ToJson(), Encoding.UTF8, "application/json");
            var request = await Api.PostAsync(Endpoint + "Register" + query, content);

            if ((int)request.StatusCode == 500)
                throw new InternalServerErrorException(await request.Content.ReadAsStringAsync());

            if ((int)request.StatusCode == 400)
                throw new BadRequestException(await request.Content.ReadAsStringAsync());

            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsAsync<SSOUser>();
        }

        public async ValueTask<Token> LoginAsync(Auth auth, string query = "")
        {
            var content = new StringContent(auth.ToJson(), Encoding.UTF8, "application/json");
            var request = await Api.PostAsync(Endpoint + "Login" + query, content);
            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsAsync<Token>();
        }

        public async Task TearDown(string ssoUserId)
        {
            var user = Database.Users
                .IgnoreQueryFilters()
                .FirstOrDefault(u => u.Id == ssoUserId);

            if(user != null)
            {
                var tokens = Database.Tokens
                    .IgnoreQueryFilters()
                    .Where(t => t.UserName == user.Id)
                    .ToList();

                var userRoles = Database.UserRoles
                    .IgnoreQueryFilters()
                    .Where(r => r.User.Id == user.Id)
                    .ToList();

                Database.Tokens.RemoveRange(tokens);
                Database.UserRoles.RemoveRange(userRoles);
                Database.Users.Remove(user);
                await Database.SaveChangesAsync();
            }
        }
    }
}
