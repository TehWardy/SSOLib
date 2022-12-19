using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Security.Data.EF;
using System.Net.Http;
using SecuritySQLite;
using SSO.AcceptanceTests;
using SharedObjects.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Security.AcceptanceTests.Exceptions;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Security.AcceptanceTests.Clients
{
	public class RegisterApiClient
	{
        readonly WebApplicationFactory<Program> webApplicationFactory;
        HttpClient api;
        public SSODbContext Database { get; set; }

        const string endpoint = "Api/Register/";

        public RegisterApiClient()
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

        public async ValueTask PostAsync(string query, object content)
        {
            var request = await api.PostAsync(endpoint + query, new StringContent(content.ToJson(), Encoding.UTF8, "application/json"));

            if ((int)request.StatusCode == 500)
                throw new InternalServerErrorException(await request.Content.ReadAsStringAsync());

            if ((int)request.StatusCode == 400)
                throw new BadRequestException(await request.Content.ReadAsStringAsync());

            request.EnsureSuccessStatusCode();
        }

        public async ValueTask<SSOUser> RegisterAsync(RegisterUser registerUser, string query = "")
        {
            var content = new StringContent(registerUser.ToJson(), Encoding.UTF8, "application/json");
            var request = await api.PostAsync(endpoint + "Register" + query, content);

            if ((int)request.StatusCode == 500)
                throw new InternalServerErrorException(await request.Content.ReadAsStringAsync());

            if ((int)request.StatusCode == 400)
                throw new BadRequestException(await request.Content.ReadAsStringAsync());

            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsAsync<SSOUser>();
        }

        public async Task TearDown(string ssoUserId)
        {
            var user = Database.Users
                .IgnoreQueryFilters()
                .FirstOrDefault(u => u.Id == ssoUserId);

            if (user != null)
            {
                var tokens = Database.Tokens
                    .IgnoreQueryFilters()
                    .Where(t => t.UserName == user.Id)
                    .ToList();

                var userRoles = Database.UserRoles
                    .IgnoreQueryFilters()
                    .Where(r => r.UserId == user.Id)
                    .ToList();

                Database.Tokens.RemoveRange(tokens);
                Database.UserRoles.RemoveRange(userRoles);
                Database.Users.Remove(user);
                await Database.SaveChangesAsync();
            }
        }
    }
}

