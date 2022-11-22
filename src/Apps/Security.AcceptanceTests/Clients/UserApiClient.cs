using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Security.AcceptanceTests.Exceptions;
using Security.Data.EF;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using SecuritySQLite;
using SharedObjects.Extensions;
using SSO.AcceptanceTests;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Security.AcceptanceTests.Clients
{
    public class UserApiClient
    {
        readonly WebApplicationFactory<Program> webApplicationFactory;
        readonly HttpClient Api;

        public SSODbContext Database { get; private set; }

        const string Endpoint = "Api/Account/";

        public UserApiClient()
        {
            webApplicationFactory = new();
            webApplicationFactory.EnsureSSOSetupForTesting();

            Api = webApplicationFactory.CreateClient();
            Api.Authenticate("TestUser@corporatelinx.com", "TestPass01!").AsTask().Wait();
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

        public async void TearDown(string ssoUserId)
        {
            var user = Database.Users
                .IgnoreQueryFilters()
                .FirstOrDefault(u => u.Id == ssoUserId);

            if(user != null)
            {
                var tokens = Database.Tokens.Where(t => t.UserName == user.Id).ToList();
                var userRoles = Database.UserRoles.Where(r => r.User.Id == user.Id).ToList();
                Database.Tokens.RemoveRange(tokens);
                Database.UserRoles.RemoveRange(userRoles);
                Database.Users.Remove(user);
                await Database.SaveChangesAsync();
            }
        }
    }
}
